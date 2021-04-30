using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace RhythmsGonnaGetYou
{
    class Band
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string CountryOfOrigin { get; set; }
        public string Website { get; set; }
        public string Style { get; set; }
        public Boolean IsSigned { get; set; }
        public string ContactName { get; set; }
        public int ContactPhoneNumber { get; set; }

    }
    class Album
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public Boolean IsExplicit { get; set; }
        public DateTime ReleaseDate { get; set; }
        public int BandId { get; set; }
        public Band Band { get; set; }
    }
    class Song
    {
        public int Id { get; set; }
        public int TrackNumber { get; set; }
        public string Title { get; set; }
        public TimeSpan Duration { get; set; }
        public int AlbumId { get; set; }
        public Album Album { get; set; }
    }
    class MyMusicLabelContext : DbContext
    {
        public DbSet<Band> Band { get; set; }

        public DbSet<Album> Album { get; set; }

        public DbSet<Song> Song { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("server=localhost;database=MyMusicLabel");
        }
    }
    class Program
    {

        static void DisplayGreeting()
        {
            Console.WriteLine("-----------------------------------------");
            Console.WriteLine(" ");
            Console.WriteLine("       Welcome to the Rhythm Cafe! ");
            Console.WriteLine(" ");
            Console.WriteLine("-----------------------------------------");
        }

        static string PromptForString(string prompt)
        {
            Console.Write(prompt);
            var userInput = Console.ReadLine();

            return userInput;
        }

        static int PromptForInteger(string prompt)
        {
            Console.Write(prompt);
            int userInput;
            var isThisGoodInput = Int32.TryParse(Console.ReadLine(), out userInput);

            if (isThisGoodInput)
            {
                return userInput;
            }
            else
            {
                Console.WriteLine("Sorry, that isn't a valid input, I'm using 0 as your answer.");
                return 0;
            }

        }
        static void Main(string[] args)
        {
            DisplayGreeting();

            var context = new MyMusicLabelContext();

            var keepGoing = true;

            while (keepGoing)
            {
                Console.WriteLine();
                Console.Write("Do you want to (V)iew, (A)dd, choose the (F)ate of a band, or (Q)uit? ");
                var choice = Console.ReadLine().ToUpper();

                if (choice == "Q")
                {
                    keepGoing = false;
                }
                else if (choice == "V")
                {
                    Console.WriteLine();
                    Console.Write("Do you want to view (A)ll, by album (R)elease Date, that are (S)igned, that are (U)nsigned or a Band and their (A)lbums? ");
                    var answer = Console.ReadLine().ToUpper();

                    if (answer == "A")
                    {
                        var bands = context.Band;
                        foreach (var band in bands)
                        {
                            Console.WriteLine($"{band.Name}");
                        }
                    }

                    if (answer == "R")
                    {
                        var whenReleased = context.Album.OrderBy(Album => Album.ReleaseDate);
                        foreach (var Album in whenReleased)
                        {
                            Console.WriteLine($"{Album.Title} released in {Album.ReleaseDate} . ");
                        }
                    }

                    if (answer == "S")
                    {
                        var signedBands = context.Band.Where(Band => Band.IsSigned);
                        foreach (var Band in signedBands)
                        {
                            Console.WriteLine($"{Band.Name} is signed. ");
                        }
                    }

                    if (answer == "U")
                    {
                        var notSigned = context.Band.Where(Band => Band.IsSigned == false);
                        foreach (var Band in notSigned)
                        {
                            Console.WriteLine($"{Band.Name} are not signed. ");
                        }
                    }

                    if (answer == "A")
                    {
                        var bandWorks = PromptForString("What band are you looking for? ");
                        MyMusicLabelContext bandContent = Band.FirstOrDefault(Band => Band.Name == bandWorks);

                        if (bandWorks == null)
                        {
                            Console.WriteLine("No bands by that name here! ");
                        }
                        else
                        {
                            Console.WriteLine($"{bandWorks}");
                        }
                    }



                    // else if ()

                }
            }




            // var Bands = context.Band;

            // var Albums = context.Album;

            // var Songs = context.Song;

            // var albumCount = context.Album.Count();
            // Console.WriteLine($"There are {albumCount} albums!");
            // var albums = context.Album.Include(album => album.Band);

            // foreach (var album in albums)
            // {
            //     if (album.Band != null)
            //     {
            //         Console.WriteLine($"There is a movie named {album.Title} by {album.Band.Name}! ");
            //     }
            // }

            // foreach (var band in Bands)
            // {
            //     Console.WriteLine($"There is a band named {band.Name}! ");
            // }

            // foreach (var song in Songs)
            // {
            //     Console.WriteLine($"There is a song named {song.Title}! ");
            // }

            // TimeSpan timeSpan = new TimeSpan(2, 14, 18);
            // Console.WriteLine(timeSpan.ToString());

        }
    }
}

