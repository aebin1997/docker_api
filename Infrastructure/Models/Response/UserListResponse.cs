namespace Infrastructure.Models.Response
{
    public class UserListResponse
    {
        public int TotalCount { get; set; }

        public List<UserList> List { get; set; }

        public UserListResponse(int totalCount, List<UserList> list)
        {
            TotalCount = totalCount;
            List = list;
        }
    }

    public class UserList
    {
        public int Idx { get; set; }

        public string UserId { get; set; } = "";

        public string UserPw { get; set; } = "";
        
        public int? LifeBestScore { get; set; }
        
        public DateTime Created { get; set; }
        
        public DateTime Updated { get; set; }
        
        public bool Deleted { get; set; }
    }
}