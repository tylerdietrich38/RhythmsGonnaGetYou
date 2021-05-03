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
        public int NumberOfMembers { get; set; }
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
                    Console.Write("Do you want to view (A)ll, by album (R)elease Date, that are (S)igned, that are (U)nsigned or a Band and their (W)orks? ");
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

                    if (answer == "W")
                    {
                        var bandWorks = PromptForString("What band are you looking for? ");
                        var bandContent = context.Band.FirstOrDefault(Band => Band.Name == bandWorks);
                        if (bandWorks == null)
                        {
                            Console.WriteLine("No bands by that name here! ");
                        }
                        else
                        {
                            Console.WriteLine($"{bandWorks} and their albums ----");

                            var works = context.Album.Include(album => album.Band);

                            var listOfAlbums = context.Album.Where(Album => Album.Band.Name == bandWorks);
                            foreach (var Album in listOfAlbums)
                            {
                                Console.WriteLine($"{Album.Title}");
                            }
                        }
                    }
                }
                else if (choice == "A")
                {
                    Console.WriteLine();
                    Console.WriteLine("Do you want add a new (B)and, (A)lbum, or (S)ong? ");
                    var answer = Console.ReadLine().ToUpper();

                    if (answer == "B")
                    {
                        var enteredBand = new Band();

                        enteredBand.Name = PromptForString("What is the Band's name? ");

                        enteredBand.CountryOfOrigin = PromptForString("What country is this band from? ");

                        enteredBand.NumberOfMembers = PromptForInteger("How many members are in the band? ");

                        enteredBand.Website = PromptForString("What is the band's website? ");

                        enteredBand.Style = PromptForString("What Genre of music does the band play? ");

                        // Need help on how to approach this below
                        Console.Write("Is the band signed? ");
                        var bandSigned = Console.ReadLine();

                        enteredBand.ContactName = PromptForString("What is the contacts name? ");

                        enteredBand.ContactPhoneNumber = PromptForInteger("What is the contacts phone number? ");

                        context.Band.Add(enteredBand);
                        context.SaveChanges();
                    }

                    else if (answer == "A")
                    {
                        var enteredAlbum = new Album();

                        enteredAlbum.Title = PromptForString("What is the Album's title?");

                        Console.Write("Is the album explicit? ");
                        var explicitAlbum = Console.ReadLine();

                        Console.WriteLine("When was the album released? ");
                        var releasedAlbum = Console.ReadLine();

                        enteredAlbum.BandId = PromptForInteger("What is the associated band's ID number? ");

                        context.Album.Add(enteredAlbum);
                        context.SaveChanges();
                    }

                    else if (answer == "S")
                    {
                        var enteredSong = new Song();

                        enteredSong.Title = PromptForString("What is the song's title? ");

                        enteredSong.TrackNumber = PromptForInteger("What is the song's track number? ");

                        TimeSpan timeSpan = new TimeSpan();
                        Console.WriteLine("What is the song's duration? ");
                        var timeSpand = Console.ReadLine();

                        enteredSong.AlbumId = PromptForInteger("What is the album's ID number? ");

                        context.Song.Add(enteredSong);
                        context.SaveChanges();
                    }
                }
                else if (choice == "F")
                {
                    Console.WriteLine();
                    Console.WriteLine("Decide the fate of a band, choose them to be (S)igned or (U)nsigned.... ");
                    var answer = Console.ReadLine().ToUpper();

                    if (answer == "S")
                    {
                        var signedBand = PromptForString("Which band is to be signed? ");
                        var bandToBeSigned = context.Band.FirstOrDefault(Band => Band.Name == signedBand);

                        if (signedBand == null)
                        {
                            Console.WriteLine("Not happening today! ");
                        }
                        else
                        {
                            Console.WriteLine($"{signedBand} is now signed! ");

                            bandToBeSigned.IsSigned = true;

                            context.SaveChanges();
                        }
                    }
                    else if (answer == "U")
                    {
                        var unsignedBand = PromptForString("Which band is to be unsigned? ");
                        var bandToBeUnSigned = context.Band.FirstOrDefault(Band => Band.Name == unsignedBand);

                        if (unsignedBand == null)
                        {
                            Console.WriteLine("Not happening today! ");
                        }
                        else
                        {
                            Console.WriteLine($"{unsignedBand} is now signed! ");

                            bandToBeUnSigned.IsSigned = false;

                            context.SaveChanges();
                        }
                    }

                }

            }
        }
    }
}


