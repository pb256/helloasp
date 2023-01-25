using System.Collections;
using ServerApp.Models;

namespace ServerApp.Services;

public static class UserService
{
    private static List<User> Users { get; }
    private static int nextId;

    static UserService()
    {
        Users = new List<User>
        {
            new() { Id = 1, Name = "철수", IsMale = true },
            new() { Id = 2, Name = "영희", IsMale = false }
        };
        nextId = Users.Count + 1;
    }

    public static List<User> GetAll() => Users;
    
    public static User? Get(int id) => Users.FirstOrDefault(x => x.Id == id);

    public static void Add(User user)
    {
        user.Id = nextId++;
        Users.Add(user);
    }

    public static void Delete(int id)
    {
        var user = Get(id);
        if (user is null) 
            return;
        Users.Remove(user);
    }

    public static void Update(User user)
    {
        var index = Users.FindIndex(x => x.Id == user.Id);
        if (index == -1)
            return;
        Users[index] = user;
    }
}