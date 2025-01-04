var Pagination = {
    settings: {
        ajax: false,
        method: 'GET',
        fetchUrl: false,
        browserPath: false,
        selector: undefined
    },

    params: {
        jqXHR: false,
    },

    init: function (settings) {
        this.settings = $.extend({}, this.settings, settings);
    },

    getData: function (method, paramsObj) {
        
        if (this.params.jqXHR && this.params.jqXHR.readyState !== 4) {
            this.params.jqXHR.abort();
        }

        if (method) {
            this.settings.method = method;
        }

        var paramsBuilder = createPaginationParamsBuilder();

        var beforePayload = { paramsBuilder };
        $(this).trigger({ type: 'before', payload: beforePayload });

        if (this.settings.method.toUpperCase() === 'GET') {
            this.setLoadWaiting(1);

            var self = this;
            var urlBuilder = createPaginationURLBuilder(this.settings.fetchUrl);

            if (paramsObj) {
                paramsBuilder.addParameters(paramsObj);
            }

            this.params.jqXHR = $.ajax({
                cache: false,
                url: urlBuilder.addParameters(paramsBuilder.build()).build(),
                type: this.settings.method,
                success: function (response) {
                    if (self.settings.selector) {
                        $(self.settings.selector).html(response);
                    }
                    $(self).trigger({ type: 'loaded' });
                },
                error: function () {
                    $(self).trigger({ type: 'error' });
                },
                complete: function () {
                    self.setLoadWaiting();
                }
            });

        } else {
            this.setLoadWaiting(1);

            if (page) {
                paramsBuilder.addParameter('PageNumber', page);
            }

            var self = this;

            this.params.jqXHR = $.ajax({
                cache: false,
                contentType: "application/json",
                url: this.settings.fetchUrl,
                type: 'POST',
                data: JSON.stringify(paramsBuilder.build()),
                success: function (response) {
                    if (self.settings.selector) {
                        $(self.settings.selector).html(response);
                    }
                    $(self).trigger({ type: 'loaded' });
                },
                error: function () {
                    $(self).trigger({ type: 'error' });
                },
                complete: function () {                   
                    self.setLoadWaiting();
                }
            });
        }
    },

    ExportDataWithPagination: function (method, paramsObj, urlObj) {
        if (this.params.jqXHR && this.params.jqXHR.readyState !== 4) {
            this.params.jqXHR.abort();
        }

        if (method) {
            this.settings.method = method;
        }

        var paramsBuilder = createPaginationParamsBuilder();

        var beforePayload = { paramsBuilder };
        $(this).trigger({ type: 'before', payload: beforePayload });

        if (this.settings.method.toUpperCase() === 'GET') {
            /*  this.setLoadWaiting(1);*/
            this.setLoadWaiting(1);

            var urlBuilder = createPaginationURLBuilder(urlObj);
            var activePageIndex = $('.page-item.active .page-link').data('page');

            paramsObj = {
                PageNumber: activePageIndex
            };        
            if (paramsObj) {
                paramsBuilder.addParameters(paramsObj);              
            }
         
            var self = this;
            this.params.jqXHR = $.ajax({
                cache: false,
                url: urlBuilder.addParameters(paramsBuilder.build()).build(),
                type: this.settings.method,
                xhrFields: {
                    responseType: "blob"
                },
                beforeSend: function () {
                    debugger
                   
                },
                success: function (response, status, xhr) {
                    debugger
                    if (xhr.status === 200) {

                        var blob = new Blob([response], { type: "application/vnd.ms-excel" });
                        var disposition = xhr.getResponseHeader('Content-Disposition');
                        if (disposition && disposition.indexOf('attachment') !== -1) {
                            var filenameRegex = /filename[^;=\n]*=((['"]).*?\2|[^;\n]*)/;
                            var matches = filenameRegex.exec(disposition);
                            if (matches != null && matches[1]) {
                                filename = matches[1].replace(/['"]/g, '');
                            }
                        }
                        if (window.navigator.msSaveOrOpenBlob) {
                            // Internet Explorer and Edge
                            window.navigator.msSaveOrOpenBlob(blob, filename);
                        } else {
                            // Other browsers
                            var link = document.createElement("a");
                            link.href = URL.createObjectURL(blob);
                            link.download = filename;
                            document.body.appendChild(link);
                            link.click();
                            document.body.removeChild(link);
                        }
                    }
                    else {
                        location.reload();
                    }
                },
                complete: function () {
                    debugger
                    self.setLoadWaiting();
                   
                }

            });

        }

    },

    setLoadWaiting: function (enable) {
        var $busyEl = $('.ajax-loading-busy');

        if (enable) {
            $busyEl.removeClass('display-none');
        } else {
            $busyEl.addClass('display-none');
        }
    }
};






function createPaginationParamsBuilder() {
    return {
        params: {},

        addParameter: function (name, value) {
            this.params[name] = value;
            return this;
        },

        addParameters: function (params) {
            this.params = $.extend({}, this.params, params);
            return this;
        },

        build: function () {
            return this.params;
        }
    };
}

function createPaginationURLBuilder(basePath) {
    return {
        params: {
            basePath: basePath,
            query: {}
        },

        addBasePath: function (url) {
            this.params.basePath = url;
            return this;
        },

        addParameter: function (name, value) {
            this.params.query[name] = value;
            return this;
        },

        addParameters: function (params) {
            this.params.query = $.extend({}, this.params.query, params);
            return this;
        },

        build: function () {
            var query = $.param(this.params.query);
            var url = this.params.basePath;

            return url.indexOf('?') !== -1
                ? url + '&' + query
                : url + '?' + query;
        }
    }
}

function addPaginationHandlers() {
    /* Common Handlers */
    addPaginationFooterHandlers();
    addPaginationHeaderHandlers();

    // when partial content is loaded through ajax
    $(Pagination).on('loaded', function () {
        if (feather) feather.replace();
        addPaginationFooterHandlers();
    });
}

// handlers for pagination header
function addPaginationHeaderHandlers() {
    $('#paginationHeaderForm').on('submit', function (e) {
        e.preventDefault();
        e.stopPropagation();
    })

    var tableFilterTimeout = null;
    $('#btnPaginationSearch').on('keyup', function (e) {
        e.preventDefault();

        if (tableFilterTimeout != null) clearTimeout(tableFilterTimeout);
        tableFilterTimeout = setTimeout(() => Pagination.getData(), 500);
    });

    $('#btnPaginationSearch').keypress(function (e) {
        let keyCode = (e.keyCode ? e.keyCode : e.which);
        if (keyCode == '13') {
            Pagination.getData();
        }
    });

    $('select[name=pageSize]').change(function (e) {
        Pagination.getData();
    });
}

// handlers for pagination footer
function addPaginationFooterHandlers() {
    // Pager
    $('.page-link').on('click', function (e) {
        e.preventDefault();

        Pagination.getData(undefined, { 'pageNumber': $(this).data('page') });
        return false;
    });

    // sorting
    $('th[data-sortorder]').click(function (e) {
        var sortBy = $(this).data('sortby');
        var sortOrder = $(this).data('sortorder');

        Pagination.getData(undefined, { 'SortBy': sortBy, 'SortOrder': sortOrder });
    });
}

// get all field params
function getPaginationFieldParams() {
    var param = {}

    //var activePageIndex = $('.page-item.active .page-link').data('page');
    //param['pageIndex'] = activePageIndex;

    // sorting
    var $activeSortByEl = $('th[data-sortbyactive="true"]');
    if ($activeSortByEl.length) {
        param['SortBy'] = $($activeSortByEl).data('sortby');

        var sortOrder = $($activeSortByEl).data('sortorder');
        param['SortOrder'] = sortOrder.toUpperCase() === 'ASC' ? 'DESC' : 'ASC';
    }

    var paginationHeaderFormValues = $('#paginationHeaderForm').serializeArray();
    $.each(paginationHeaderFormValues, function (i, obj) {
        param[obj['name']] = obj['value'];
    });

    return param;
}