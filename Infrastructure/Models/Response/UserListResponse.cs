namespace Infrastructure.Models.Response
{
    public class UserListResponse
    {
        private int TotalCount { get; set; }

        private List<UserList> List { get; set; }

        public UserListResponse(int totalCount, List<UserList> list)
        {
            this.TotalCount = totalCount;
            this.List = list;
        }
    }

    public class UserList
    {
        public int Idx { get; set; }

        public string UserId { get; set; } = "";

        public string UserPw { get; set; } = "";
        
        public int? LifeBestScore { get; set; }
        
        public DateTime Created { get; set; }
        
        public bool Deleted { get; set; }
    }
}