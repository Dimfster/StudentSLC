using StudentSLC.Models;
using StudentSLC.Data;
using Microsoft.EntityFrameworkCore;
using StudentSLC.Security;

public static class SeedData
{
    public static async Task Initialize(AppDbContext _db, PasswordHasher _passwordHasher)
    {
        if (await _db.Users.AnyAsync()) return; 

        var studentRole = new Role { Name = "student" };
        var teacherRole = new Role { Name = "teacher" };
        var keyholderRole = new Role { Name = "keyholder" };
        var adminRole = new Role { Name = "admin" };

        _db.Roles.AddRange(studentRole, teacherRole, keyholderRole, adminRole);


        var groupA = new Group { Name = "Group A", Users = new List<User>(), Participant = new Participant { Type = "group" } };
        var groupB = new Group { Name = "Group B", Users = new List<User>(), Participant = new Participant { Type = "group" } };
        var groupC = new Group { Name = "Group C", Users = new List<User>(), Participant = new Participant { Type = "group" } };

        _db.Groups.AddRange(groupA, groupB, groupC);

        var users = new List<User>();
        for (int i = 1; i <= 20; i++)
        {
            var participant = new Participant { Type = "user" };

            var user = new User
            {
                FirstName = $"First{i}",
                LastName = $"Last{i}",
                Patronymic = $"Pat{i}",
                Role = "student",
                UserCode = 100000 + i,
                PasswordHash = _passwordHasher.Generate("12345"),
                Participant = participant
            };

            users.Add(user);
        }

        users[0].Role = "admin";

        for (int i = 1; i <= 3; i++)
        {
            users[i].Role = "teacher";   
            users[i].Role = "keyholder";
        }

        for (int i = 3; i < 20; i++)
        {
            if (i % 3 == 0) groupA.Users.Add(users[i]);
            else if (i % 3 == 1) groupB.Users.Add(users[i]);
            else groupC.Users.Add(users[i]);
        }

        var rooms = new List<Room>
        {
            new Room { Name = "Сцена ЦДС", Type = "Зал" },
            new Room { Name = "Комната звукозаписи", Type = "Класс" },
            new Room { Name = "Комната для репитиции 101",   Type = "Класс" },
            new Room { Name = "Комната для репитиции 102",   Type = "Класс" },
            new Room { Name = "Сцена театра", Type = "Зал" }
        };
        _db.Rooms.AddRange(rooms);

        var times = new[]
        {
            new TimeSpan(8,30,0),
            new TimeSpan(10,15,0),
            new TimeSpan(11,45,0),
            new TimeSpan(12,0,0),
            new TimeSpan(14,0,0)
        };

        // понедельник текущей недели
        DateTime now = DateTime.UtcNow;
        int diff = (7 + (now.DayOfWeek - DayOfWeek.Monday)) % 7;
        DateTime weekStart = now.Date.AddDays(-diff);

        var events = new List<Event>();
        int eventCounter = 1;

        for (int day = 0; day < 5; day++) // понедельник-пятница
        {
            foreach (var t in times)
            {
                if (events.Count == 10) break;

                var start = weekStart.AddDays(day).Add(t);
                var end = start.AddMinutes(90); 

                var ev = new Event
                {
                    Name = $"Event {eventCounter}",
                    StartTime = start,
                    EndTime = end,
                    RoomName = rooms[eventCounter % rooms.Count].Name,
                    Room = rooms[eventCounter % rooms.Count]
                };

                // участники: случайные студенты одной группы
                ev.Participants.Add(users[3 + (eventCounter % 15)].Participant);

            
                ev.KeyHolders.Add(users[1].Participant);
                ev.KeyHolders.Add(users[2].Participant);

                events.Add(ev);
                eventCounter++;
            }
        }

        _db.Users.AddRange(users);
        _db.Events.AddRange(events);

        await _db.SaveChangesAsync();
    }
}
