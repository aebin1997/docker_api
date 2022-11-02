namespace Infrastructure.Models.Request
{
    public class UpdateUserRequest
    {
        public string ColumnName { get; set; }
    
        public string DataBefore { get; set; }
    
        public string DataAfter { get; set; }
    }

    public class UpdateUserParameterModel
    {
        public int Idx { get; set; }
        public string UserId { get; set; } = "";
        
        public string UserPw { get; set; } = "";
        
        public int? LifeBestScore { get; set; }
    }
}