namespace Mpmt.Core.Dtos.Pagination
{
    /// <summary>
    /// The pager model.
    /// </summary>
    public class PagerModel : ActionSpecs
    {
        /// <summary>
        /// The _no of pager buttons.
        /// </summary>
        private const int _noOfPagerButtons = 5;
        private readonly int _noOfButtonsToTheLeftFromCurrentPagerBtn = (int)Math.Ceiling((decimal)_noOfPagerButtons / 2);
        private readonly int _noOfButtonsToTheRightFromCurrentPagerBtn = (int)Math.Ceiling((decimal)_noOfPagerButtons / 2) - 1;

        /// <summary>
        /// Gets or sets the start page.
        /// </summary>
        public int StartPage { get; set; }
        /// <summary>
        /// Gets or sets the current page.
        /// </summary>
        public int CurrentPage { get; set; }
        /// <summary>
        /// Gets or sets the end page.
        /// </summary>
        public int EndPage { get; set; }
        /// <summary>
        /// Gets or sets the total pages.
        /// </summary>
        public int TotalPages { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PagerModel"/> class.
        /// </summary>
        /// <param name="totalPages">The total pages.</param>
        /// <param name="pageIndex">The page index.</param>
        /// <param name="controller">The controller.</param>
        /// <param name="action">The action.</param>
        public PagerModel(int totalPages, int pageIndex, string controller, string action)
        {
            TotalPages = totalPages;
            CurrentPage = pageIndex;

            int startPage = CurrentPage - _noOfButtonsToTheLeftFromCurrentPagerBtn;
            int endPage = CurrentPage + _noOfButtonsToTheRightFromCurrentPagerBtn;

            if (startPage <= 0)
            {
                endPage -= startPage - 1;
                startPage = 1;
            }

            if (endPage > totalPages)
            {
                endPage = totalPages;
                if (endPage > _noOfPagerButtons)
                {
                    startPage = endPage - _noOfPagerButtons - 1;
                }
            }

            TotalPages = totalPages;
            StartPage = startPage;
            EndPage = endPage;

            Controller = controller;
            Action = action;
        }

        /// <summary>
        /// Gets a value indicating whether first is page.
        /// </summary>
        public bool IsFirstPage => CurrentPage == 1;

        /// <summary>
        /// Gets a value indicating whether has previous page.
        /// </summary>
        public bool HasPreviousPage => CurrentPage > 1;

        /// <summary>
        /// Gets a value indicating whether has next page.
        /// </summary>
        public bool HasNextPage => CurrentPage < TotalPages;

        /// <summary>
        /// Gets a value indicating whether last is page.
        /// </summary>
        public bool IsLastPage => CurrentPage == TotalPages;
    }
}
