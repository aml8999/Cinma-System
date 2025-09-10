using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using System.Threading.Tasks;


// ------------------ ExceptionHandling ------------------
class ExceptionHandling : Exception                                  //     هنا عملت كلاس Exception  عشان احط فيه الدوال اللى بتتعامل مع الاستثناءات
{
    //هنا انا فكرت بدل ملا نعمل try&catch فى كل ادخال نعمل دوال لكل انواع الاكسبشن الممكن توقع البرنامج ونستدعيها عند الادخال  حسب كل ادخال طبعا ونوفر التكرار والكود يبقى مرن اكتر
    public static int ValidInt(string prompt, int? min = null, int? max = null)

    //هنا بتاكد ان انا ادخلت رقم صحيح بين رقمين اختياريين
    {
        int value;
        while (true)
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write(prompt);
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.Yellow;
            var s = Console.ReadLine();
            Console.ResetColor();

            if (int.TryParse(s, out value))
            {
                if ((min == null || value >= min) && (max == null || value <= max))
                    return value;

                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Please enter number between {min} and {max}.");
                Console.ResetColor();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Invalid input!!..Please enter a number.");
                Console.ResetColor();
            }
        }
    }

    public static string NonEmptyString(string prompt)   //بتاكد ان المستخدم مسبش الحقل فاضى   
    {
        string input;
        do
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write(prompt);
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.Yellow;
            input = Console.ReadLine()?.Trim();
            Console.ResetColor();

            if (string.IsNullOrEmpty(input))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Input cannot be empty.");
                Console.ResetColor();
            }
        } while (string.IsNullOrEmpty(input));

        return input;
    }
}

// ------------------ Role enum ------------------
public enum Role
{
    None = 0,
    Admin,
    Customer,
}

// ------------------ Genre helper ------------------
public static class Genre                               // انواع الافلام 
{
    private static readonly string[] Genres =
    {
        "Horror",
        "Action",
        "Comedy",
        "Drama",
        "Romance"
    };


    public static string ChooseGenre()             // ابختار نوع الفيلم
    {
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.WriteLine("\nChoose a genre:");
        for (int i = 0; i < Genres.Length; i++)
            Console.WriteLine($"{i + 1}. {Genres[i]}");
        Console.ResetColor();

        int choice = ExceptionHandling.ValidInt("Enter choice: ", 1, Genres.Length);
        return Genres[choice - 1];
    }
}

// ------------------ Ticket ------------------
public class Ticket
{
    public string FilmName { get; }
    public int SeatNumber { get; }
    public int HallNumber { get; }
    public DateTime BookedAt { get; }

    public Ticket(string filmName, int seatNumber, int hallNumber)
    {
        FilmName = filmName;
        SeatNumber = seatNumber;
        HallNumber = hallNumber;
        BookedAt = DateTime.Now;
    }

    public void PrintTicket() //هنا بطبع التذكره
    {
        int width = 40;
        string border = new string('*', width);

        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine(border);
        Console.WriteLine($"*{CenterText("CINEMA TICKET", width - 2)}*");
        Console.WriteLine(border);
        Console.WriteLine($"* Film : {PadRight(FilmName, width - 9)}*");
        Console.WriteLine($"* Seat : {PadRight(SeatNumber.ToString(), width - 9)}*");
        Console.WriteLine($"* Hall : {PadRight(HallNumber.ToString(), width - 9)}*");
        Console.WriteLine($"* Date : {PadRight(BookedAt.ToString("yyyy-MM-dd HH:mm"), width - 9)}*");
        Console.WriteLine(border);
        Console.ResetColor();
        Console.WriteLine();
    }


    static string CenterText(string s, int width)   // داله توسط العنوان فى النص بلظبط
    {
        if (s.Length >= width) return s.Substring(0, width);
        int left = (width - s.Length) / 2;
        return new string(' ', left) + s + new string(' ', width - s.Length - left);
    }

    static string PadRight(string s, int width)
    {
        if (s.Length >= width) return s.Substring(0, width);
        return s + new string(' ', width - s.Length);
    }


}

// ------------------ Register ------------------
public class Register
{
    public Role UserRole { get; set; } // لازم يبقى set عشان DataManager
    public string FullName { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }



    public void RegisterUser(string usernameInput, string password) //هنا بتاكد ان انا سجلت المستخدم
    {

        bool isAdmin = usernameInput.EndsWith("#");
        string normalized = isAdmin ? usernameInput.TrimEnd('#') : usernameInput;
        UserRole = isAdmin ? Role.Admin : Role.Customer;
        UserName = normalized.Trim();
        Password = password.Trim();

    }

    public void InputData(List<Register> registers)
    {
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.WriteLine("\n=== Register new user ===");
        Console.ResetColor();

        string fullName = ExceptionHandling.NonEmptyString("Enter your full name: ");
        string username = ExceptionHandling.NonEmptyString("Enter Username (add '#' at end if Admin): ");
        string password = ExceptionHandling.NonEmptyString("Enter Password: ");

        FullName = fullName;
        RegisterUser(username, password);

        registers.Add(this);     //انا بخزن كل المستخدمين فلسته 

        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine($" {(UserRole == Role.Admin ? "Admin" : "User")} \"{FullName}\" registered successfully!");
        Console.ResetColor();
    }

    public static void DisplayUsers(List<Register> registers)  // عرض بيانات العميل 

    {
        if (registers.Count == 0)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("No users registered yet.");
            Console.ResetColor();
            return;
        }

        foreach (var r in registers)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"   Name: {r.FullName}");
            Console.WriteLine($"   Username: {r.UserName}");
            Console.WriteLine($"   Role    : {r.UserRole}");
            Console.WriteLine("------------------------------");
        }
        Console.ResetColor();
    }
}

// ------------------ LogIn ------------------
public class LogIn
{
    public Register InputLogin(List<Register> registers)
    {
        string loginUserName = ExceptionHandling.NonEmptyString("Enter username: ");
        string loginPassword = ExceptionHandling.NonEmptyString("Enter password: ");
        string normalized = loginUserName.EndsWith("#") ? loginUserName.TrimEnd('#') : loginUserName;

        // var user = registers.FirstOrDefault  (r => string.Equals(r .UserName ,normalized, StringComparison.Ordinal) &&  string. Equals(r.  Password, loginPassword, StringComparison.Ordinal));


        Register user = null;
        foreach (var r in registers) //هنا بعمل لوب على كل المستخدمين المسجلين عشان اشوف هل فى حد مطابق مع اليوزر نيم والباسورد اللى دخلهم المستخدم ولا لا
        {
            bool nameOk = string.Equals(r.UserName, normalized, StringComparison.Ordinal);
            bool passwordOk = string.Equals(r.Password, loginPassword, StringComparison.Ordinal);
            if (nameOk && passwordOk)
            {
                user = r;
                break; //لو لقيته طابق مستخدم عندك  وقف 
            }

        }


        if (user != null)
        {

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"Login successful! Role: {user.UserRole}");
            Console.ResetColor();

            return user; //    لقيت اليوزر ارجع خلاص ومتكملش الداله 
        }

        // لو المستخدم بيانته مش مطابقه لبيانات اى مستخدم 
        Console.ForegroundColor = ConsoleColor.Red;

        Console.WriteLine("Invalid username or password.");
        Console.ResetColor();
        return null;
    }
}

// ------------------ Movie ------------------
public class Movie
{
    private const int START_SEAT_NUMBER = 15;
    private const int SEAT_COUNT = 41;

    public string Title { get; }
    public string Genre { get; }
    public int HallNumber { get; }
    private bool[] seats;

    public Movie(string title, string genre, int hallNumber)
    {
        Title = title;
        Genre = genre;
        HallNumber = hallNumber;
        seats = new bool[SEAT_COUNT];
    }



    public int BookSeat()    //هنا بتكاد ان انا حجزت مقعد
    {
        for (int i = 0; i < seats.Length; i++)
        {
            if (!seats[i])
            {


                seats[i] = true; // علامة ان المقعد اتحجز
                return START_SEAT_NUMBER + i;
            }
        }
        return -1;
    }

    public int AvailableSeatsCount()
    {
        return seats.Count(s => !s);    //هنا بترجع عدد المقاعد المتاحه
    }
}

// ------------------ TicketData & DataManager ------------------
public class TicketData
{
    public string FilmName { get; set; }
    public int SeatNumber { get; set; }
    public int HallNumber { get; set; }
    public string UserName { get; set; }
    public DateTime BookedAt { get; set; }

    public TicketData(string filmName, int seatNumber, int hallNumber, string userName, DateTime bookedAt)
    {
        FilmName = filmName;
        SeatNumber = seatNumber;
        HallNumber = hallNumber;
        UserName = userName;
        BookedAt = bookedAt;
    }
}


public static class DataManager
{
    private static string moviesFile = "Movies.txt";

    private static string ticketsFile = "Tickets.txt";

    private static string registersFile = "Registers.txt";

    public static void SaveMovies(List<Movie> movies)
    {
        List<string> lines = new List<string>();
        foreach (var movie in movies)
        {

            lines.Add($"{movie.Title}-{movie.Genre}-{movie.HallNumber}-{movie.AvailableSeatsCount()}");


        }
        File.WriteAllLines(moviesFile, lines);  //هنا بتكاد ان انا حفظت الافلام فى ملف
    }


    public static List<Movie> LoadMovies()
    {
        List<Movie> movies = new List<Movie>();
        if (File.Exists(moviesFile))
        {
            var lines = File.ReadAllLines(moviesFile);
            foreach (var line in lines)
            {
                var part = line.Split('-');
                if (part.Length >= 3)
                {
                    string title = part[0];
                    string genre = part[1];
                    int hall = int.Parse(part[2]);
                    movies.Add(new Movie(title, genre, hall));
                }
            }
        }
        return movies;
    }


    public static void SaveRegisters(List<Register> registers)
    {
        List<string> lines = new List<string>();
        foreach (var register in registers)
        {
            lines.Add($"{register.FullName}-{register.UserName}-{register.Password}-{register.UserRole}");
        }
        File.WriteAllLines(registersFile, lines);
    }


    public static List<Register> LoadRegisters()
    {
        List<Register> registers = new List<Register>();
        if (File.Exists(registersFile))
        {
            var lines = File.ReadAllLines(registersFile);
            foreach (var line in lines)
            {
                var part = line.Split('-');
                if (part.Length == 4)
                {
                    registers.Add(new Register
                    {
                        FullName = part[0],
                        UserName = part[1],
                        Password = part[2],
                        UserRole = Enum.TryParse(part[3], out Role role) ? role : Role.Customer
                    });
                }
            }
        }
        return registers;
    }
}


/// 



class Program
{

    //--------Layer of Abstraction----------
    static void HandleRegister(List<Register> registers)
    {
        var reg = new Register();
        reg.InputData(registers);

    }

    static Register HandleLogin(LogIn loginService, List<Register> registers)
    {
        return loginService.InputLogin(registers);
    }

    static void HandleDisplayUsers(List<Register> registers)
    {
        Register.DisplayUsers(registers);
    }


    // ------- Add movie (Admin only) -------
    static void HandleAddMovie(List<Movie> movies, Register currentUser, Random rng, int MAX_HALLS)
    {
        if (currentUser == null || currentUser.UserRole != Role.Admin)                          //هنا بتاكد ان المستخدم اللى عايز يضيف فيلم هو ادمن مش يوزر عادى
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Only Admins can add movies. Please login as Admin (username ending with '#').");
            Console.ResetColor();
            return;
        }

        string movieTitle = ExceptionHandling.NonEmptyString("Movie title: ");
        string genre = Genre.ChooseGenre();
        int hall = rng.Next(1, MAX_HALLS + 1);
        movies.Add(new Movie(movieTitle, genre, hall));

        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine($"Movie '{movieTitle}' added in hall {hall}");
        Console.ResetColor();
    }



    // ------- Show available movies  -------
    static void HandleShowMovies(List<Movie> movies)
    {
        if (movies.Count == 0)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("No movies available.");
            Console.ResetColor();
            return;
        }

        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("\nAvailable movies:");
        for (int i = 0; i < movies.Count; i++)
        {
            var m = movies[i];
            Console.WriteLine($"{i + 1}. {m.Title} [{m.Genre}] - Hall {m.HallNumber} (Available seats: {m.AvailableSeatsCount()})");
        }
        Console.ResetColor();
    }


    // ------- Book a seat  -------
    static Ticket HandleBookSeat(List<Movie> movies)
    {
        if (movies.Count == 0)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("No movies to book.");
            Console.ResetColor();
            return null;
        }

        Console.ForegroundColor = ConsoleColor.Blue;
        Console.WriteLine("\nChoose movie to book (seat assigned automatically):");
        for (int i = 0; i < movies.Count; i++)
            Console.WriteLine($"{i + 1}. {movies[i].Title} [{movies[i].Genre}] - Hall {movies[i].HallNumber} (Avail: {movies[i].AvailableSeatsCount()})");
        Console.ResetColor();

        int movieChoice = ExceptionHandling.ValidInt("Choose movie (number): ", 1, movies.Count);
        var selected = movies[movieChoice - 1];

        int assignedSeat = selected.BookSeat();
        if (assignedSeat == -1)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Sorry, this movie is fully booked.");
            Console.ResetColor();
            return null;
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"Seat assigned: {assignedSeat} (Hall {selected.HallNumber})");
            Console.ResetColor();
            return new Ticket(selected.Title, assignedSeat, selected.HallNumber);
        }
    }


    // ------- Print ticket  -------                              //هنا بتكاد ان انا حجزت تزكره فى الاصل        
    static void HandlePrintTicket(Ticket lastTicket)
    {
        if (lastTicket != null)
            lastTicket.PrintTicket();
        else
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("No ticket booked yet.");
            Console.ResetColor();
        }
    }



    // ------- Exit handler -------
    static void HandleExit(ref bool exit)
    {
        exit = true;
        Console.ForegroundColor = ConsoleColor.Yellow;

        Console.WriteLine("thank you for using the system...");
        Console.ResetColor();
    }






    static void Main()
    {


        // Console.OutputEncoding = System.Text.Encoding.UTF8;  //الترميز الأشهر اللي بيدعم معظم لغات العالم (العربية، الإنجليزية، الإيموجي، )

        string title = " Cinema Ticket Management System ";
        string border = new string('=', title.Length + 6);

        Console.ForegroundColor = ConsoleColor.Yellow;

        Console.WriteLine(border);
        Console.WriteLine($"==  {title}  ==");
        Console.WriteLine(border);
        Console.ResetColor();

        var registers = DataManager.LoadRegisters();     //بياخد المستخدمين المسجلين فى الملف ويخزنهم  
        var movies = DataManager.LoadMovies();          //نفس الكلام 

        var loginService = new LogIn();
        Register currentUser = null;
        Ticket lastTicket = null;
        bool exit = false;
        var rng = new Random();
        const int MAX_HALLS = 5;


        while (!exit)
        {
            try
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine("\n1. Register");

                Console.WriteLine("2. Login");

                Console.WriteLine("3. Display Users");

                Console.WriteLine("4. Add Movie (Admin only)");

                Console.WriteLine("5. Show Movies");

                Console.WriteLine("6. Book a Seat (auto-assign)");

                Console.WriteLine("7. Print Last Ticket");

                Console.WriteLine("8. Exit");

                Console.ResetColor();

                int choice = ExceptionHandling.ValidInt("Enter your choice: ", 1, 8);

                switch (choice)
                {
                    case 1:

                        HandleRegister(registers);
                        break;

                    case 2:
                        currentUser = HandleLogin(loginService, registers);

                        break;

                    case 3:
                        HandleDisplayUsers(registers);

                        break;

                    case 4:

                        HandleAddMovie(movies, currentUser, rng, MAX_HALLS);

                        break;
                    case 5:

                        HandleShowMovies(movies);

                        break;
                    case 6:

                        lastTicket = HandleBookSeat(movies);

                        break;

                    case 7:

                        HandlePrintTicket(lastTicket);
                        break;

                    case 8:
                        HandleExit(ref exit);


                        break;
                }
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Error: {ex.Message}");
                Console.ResetColor();
            }


            // Save after each action
            DataManager.SaveRegisters(registers);
            DataManager.SaveMovies(movies);


        }
    }
}









