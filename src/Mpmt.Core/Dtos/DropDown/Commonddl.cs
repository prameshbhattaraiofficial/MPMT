namespace Mpmt.Core.Dtos.DropDown
{


    /// <summary>
    /// The commonddl.
    /// </summary>
    /// 
    public class DocumentTypeddl : Commonddl
    {   

        public int IsExpirable { get; set; }
    }


    public class Commonddl
    {

        public string Text { get; set; }
        public string value { get; set; }
        public string lookup { get; set; }
    }

    public class Commondropdown
    {
        public string Text { get; set; }
        public string lookup { get; set; }
        public string value { get; set; }
    }
    public class AssignUserRoleDto
    {
        public int user_id { get; set; }
        public int[] roleid { get; set; }
    }
    public class AssignNotificationModuleRoleDto
    {
        public int moduleid { get; set; }
        public int[] roleid { get; set; }   
    }
    public class Selecteditem
    {

        public string Text { get; set; }
        public string value { get; set; }
        public bool IsSelected { get; set; }
    }

    public class GetDocumentCharcterModel
    {
        public string isExpirable { get;set; }
        public string isBackImageRequired { get; set; }
    }
}
