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
    }
    class Song
    {
        public int Id { get; set; }
        public int TrackNumber { get; set; }
        public string Title { get; set; }
        public TimeSpan Duration { get; set; }
    }
    class MyMusicLabelContext : DbContext
    {
        public DbSet<Band> Bands { get; set; }

        public DbSet<Album> Albums { get; set; }

        public DbSet<Song> Songs { get; set; }

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

        static void Main(string[] args)
        {
            DisplayGreeting();

            var context = new MyMusicLabelContext();

            var Bands = context.Bands;

            var Albums = context.Albums;

            var Songs = context.Songs;

            var bandCount = context.Bands.Count();
            Console.WriteLine($"There are {bandCount} bands!");




            // TimeSpan timeSpan = new TimeSpan(2, 14, 18);
            // Console.WriteLine(timeSpan.ToString());

        }
    }
}
