namespace Mpmt.Core.Dtos.Roles
{
    /// <summary>
    /// The partner role.
    /// </summary>
    public class PartnerRole
    {
    
        public char Event { get; set; }
        
        public int Id { get; set; }
        
        public string RoleName { get; set; }
       
        public string Description { get; set; }
        
        public bool IsSystemRole { get; set; }
        
        public bool IsActive { get; set; }
      
        public bool IsDeleted { get; set; }
       
        public string LoggedInUser { get; set; }
       
        public string UserType { get; set; }
        
        public string PartnerCode { get; set; }
    }
}
