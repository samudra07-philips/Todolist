

namespace todolist_mvvm.Bussiness_Layer
{
    public static class CurrentUser
    {
        public static int Id { get; set; }
        public static string Username { get; set; }

        public static void SetUser(int id, string username)
        {
            Id = id;
            Username = username;
        }

        public static void Clear()
        {
            Id = 0;
            Username = null;
        }

        public static bool IsAuthenticated => Id > 0;
    }

}
