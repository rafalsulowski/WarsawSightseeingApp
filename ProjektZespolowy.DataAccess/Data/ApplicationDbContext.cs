using Azure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Newtonsoft.Json;
using ProjektZespolowy.Models;
using ProjektZespolowy.Models.DTO;
using ProjektZespolowy.Models.JoinedTables;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ProjektZespolowy.DataAccess.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<Trip> Trips { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Place> Places { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<CommentPlace> CommentPlace { get; set; }
        public DbSet<PostLikes> PostLikes { get; set; }
        public DbSet<CommentLikes> CommentLikes { get; set; }
        public DbSet<CommentPlaceLikes> CommentPlaceLikes { get; set; }
        public DbSet<PlaceLikes> PlaceLikes { get; set; }
        public DbSet<PlacesTrips> PlacesTrips { get; set; }
        public DbSet<PlaceAvailabilityTime> PlaceAvailabilityTimes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            //var valueComparer = new ValueComparer<List<int>>(
            //    (c1, c2) => c1.SequenceEqual(c2),
            //    c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
            //    c => c.ToList());
            //modelBuilder.Entity<User>()
            //    .Property(s => s.LikedCommentsOfPlaces)
            //    .HasConversion(v => JsonConvert.SerializeObject(v),
            //        v => JsonConvert.DeserializeObject<List<int>>(v))
            //    .Metadata
            //    .SetValueComparer(valueComparer);


            #region User
            modelBuilder.Entity<User>()
                .HasKey(e => e.Id);

            modelBuilder.Entity<User>()
                .Property(p => p.Id)
                .ValueGeneratedOnAdd();

            //realcje one-to-many do list z elementami ktore user utworzyl
            modelBuilder.Entity<User>()
                .HasMany(sc => sc.Trips)
                .WithOne(s => s.User)
                .OnDelete(DeleteBehavior.NoAction)
                .HasForeignKey(sc => sc.UserId)
                .IsRequired();

            modelBuilder.Entity<User>()
                .HasMany(sc => sc.Posts)
                .WithOne(s => s.User)
                .HasForeignKey(sc => sc.UserId)
                .OnDelete(DeleteBehavior.NoAction)
                .IsRequired();

            modelBuilder.Entity<User>()
                .HasMany(u => u.CommentsPlace)
                .WithOne()
                .HasForeignKey(u => u.UserId)
                .OnDelete(DeleteBehavior.NoAction)
                .IsRequired();

            modelBuilder.Entity<User>()
                .HasMany(sc => sc.Comments)
                .WithOne()
                .HasForeignKey(sc => sc.UserId)
                .OnDelete(DeleteBehavior.NoAction)
                .IsRequired();

            // koniec realcji

            modelBuilder.Entity<User>()
                .Property(s => s.Username)
                .IsRequired();

            modelBuilder.Entity<User>()
                .Property(s => s.PasswordHash)
                .IsRequired();

            modelBuilder.Entity<User>()
                .Property(s => s.Email)
                .IsRequired();
            #endregion


            #region Likes
            modelBuilder.Entity<PostLikes>().HasKey(sc => new { sc.PostLikeId, sc.UserLikeId });

            modelBuilder.Entity<PostLikes>()
                .HasOne(sc => sc.Post)
                .WithMany(s => s.Likes)
                .OnDelete(DeleteBehavior.NoAction)
                .HasForeignKey(sc => sc.PostLikeId);

            modelBuilder.Entity<PostLikes>()
                .HasOne(sc => sc.User)
                .WithMany(s => s.LikedPosts)
                .OnDelete(DeleteBehavior.NoAction)
                .HasForeignKey(sc => sc.UserLikeId);

            modelBuilder.Entity<PostLikes>()
                .Property(e => e.IsLike)
                .IsRequired();


            modelBuilder.Entity<TripLikes>().HasKey(sc => new { sc.TripLikeId, sc.UserLikeId });

            modelBuilder.Entity<TripLikes>()
                .HasOne(sc => sc.Trip)
                .WithMany(s => s.Likes)
                .OnDelete(DeleteBehavior.NoAction)
                .HasForeignKey(sc => sc.TripLikeId);

            modelBuilder.Entity<TripLikes>()
                .HasOne(sc => sc.User)
                .WithMany(s => s.LikedTrips)
                .OnDelete(DeleteBehavior.NoAction)
                .HasForeignKey(sc => sc.UserLikeId);

            modelBuilder.Entity<TripLikes>()
                .Property(e => e.IsLike)
                .IsRequired();


            modelBuilder.Entity<CommentPlaceLikes>().HasKey(sc => new { sc.CommentPlaceLikeId, sc.UserLikeId });

            modelBuilder.Entity<CommentPlaceLikes>()
                .HasOne(sc => sc.CommentPlace)
                .WithMany(s => s.Likes)
                .OnDelete(DeleteBehavior.NoAction)
                .HasForeignKey(sc => sc.CommentPlaceLikeId);

            modelBuilder.Entity<CommentPlaceLikes>()
                .HasOne(sc => sc.User)
                .WithMany(s => s.LikedCommentsPlace)
                .OnDelete(DeleteBehavior.NoAction)
                .HasForeignKey(sc => sc.UserLikeId);

            modelBuilder.Entity<CommentPlaceLikes>()
                .Property(e => e.IsLike)
                .IsRequired();


            modelBuilder.Entity<CommentLikes>().HasKey(sc => new { sc.CommentLikeId, sc.UserLikeId });

            modelBuilder.Entity<CommentLikes>()
                .HasOne(sc => sc.Comment)
                .WithMany(s => s.Likes)
                .OnDelete(DeleteBehavior.NoAction)
                .HasForeignKey(sc => sc.CommentLikeId);

            modelBuilder.Entity<CommentLikes>()
                .HasOne(sc => sc.User)
                .WithMany(s => s.LikedComments)
                .OnDelete(DeleteBehavior.NoAction)
                .HasForeignKey(sc => sc.UserLikeId);

            modelBuilder.Entity<CommentLikes>()
                .Property(e => e.IsLike)
                .IsRequired();


            modelBuilder.Entity<PlaceLikes>().HasKey(sc => new { sc.PlaceLikeId, sc.UserLikeId });

            modelBuilder.Entity<PlaceLikes>()
                .HasOne(sc => sc.Place)
                .WithMany(s => s.Likes)
                .OnDelete(DeleteBehavior.NoAction)
                .HasForeignKey(sc => sc.PlaceLikeId);

            modelBuilder.Entity<PlaceLikes>()
                .HasOne(sc => sc.User)
                .WithMany(s => s.LikedPlaces)
                .OnDelete(DeleteBehavior.NoAction)
                .HasForeignKey(sc => sc.UserLikeId);

            modelBuilder.Entity<PlaceLikes>()
                .Property(e => e.IsLike)
                .IsRequired();
            #endregion


            #region PlacesTrips N-N
            modelBuilder.Entity<PlacesTrips>().HasKey(sc => new { sc.PlaceId, sc.TripId });

            modelBuilder.Entity<PlacesTrips>()
                .Property(e => e.Sequence)
                .IsRequired();

            modelBuilder.Entity<PlacesTrips>()
                .Property(e => e.TimeForPlace)
                .IsRequired();

            modelBuilder.Entity<PlacesTrips>()
                .Property(e => e.BudgetForPlace)
                .IsRequired();
            
            //modelBuilder.Entity<PlacesTrips>()
            //    .HasOne(sc => sc.Place)
            //    .WithMany(s => s.TripsThatIncludeThisPlace)
            //    .OnDelete(DeleteBehavior.NoAction)
            //    .HasForeignKey(sc => sc.TripId);

            //modelBuilder.Entity<PlacesTrips>()
            //    .HasOne(sc => sc.Trip)
            //    .WithMany(s => s.Places)
            //    .OnDelete(DeleteBehavior.NoAction)
            //    .HasForeignKey(sc => sc.PlaceId);
            #endregion


            #region CommentPlace

            modelBuilder.Entity<CommentPlace>()
                .HasKey(e => e.Id);

            modelBuilder.Entity<CommentPlace>()
                .Property(p => p.Id)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<CommentPlace>()
                .Property(s => s.Content)
                .IsRequired();

            modelBuilder.Entity<CommentPlace>()
                .Property(s => s.Note)
                .IsRequired();

            modelBuilder.Entity<CommentPlace>()
                .Property(s => s.Author)
                .IsRequired();

            //modelBuilder.Entity<CommentPlace>()
            //    .Property(s => s.Likes)
            //    .HasConversion(v => JsonConvert.SerializeObject(v),
            //        v => JsonConvert.DeserializeObject<List<int>>(v))
            //    .Metadata
            //    .SetValueComparer(valueComparer);

            //modelBuilder.Entity<CommentPlace>()
            //    .Property(s => s.DisLikes)
            //    .HasConversion(v => JsonConvert.SerializeObject(v),
            //        v => JsonConvert.DeserializeObject<List<int>>(v))
            //    .Metadata
            //    .SetValueComparer(valueComparer);            
            #endregion


            #region Comment
            modelBuilder.Entity<Comment>()
                .HasKey(e => e.Id);

            modelBuilder.Entity<Comment>()
                .Property(p => p.Id)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<Comment>()
                .Property(s => s.Content)
                .IsRequired();

            modelBuilder.Entity<Comment>()
                .Property(s => s.Author)
                .IsRequired();
            #endregion


            #region Post
            modelBuilder.Entity<Post>()
                .HasKey(e => e.Id);

            modelBuilder.Entity<Post>()
                .Property(p => p.Id)
                .ValueGeneratedOnAdd();

            //realcje
            modelBuilder.Entity<Post>()
                .HasMany(sc => sc.Comments)
                .WithOne()
                .OnDelete(DeleteBehavior.NoAction)
                .HasForeignKey(sc => sc.PostId)
                .IsRequired();
            //

            modelBuilder.Entity<Post>()
                .Property(s => s.Title)
                .IsRequired();

            modelBuilder.Entity<Post>()
                .Property(s => s.Content)
                .IsRequired();

            modelBuilder.Entity<Post>()
                .Property(s => s.Type)
                .IsRequired();

            modelBuilder.Entity<Post>()
                .Property(s => s.VotesFor)
                .IsRequired();

            modelBuilder.Entity<Post>()
                .Property(s => s.VotesAgainst)
                .IsRequired();

            modelBuilder.Entity<Post>()
                .Property(s => s.Author)
                .IsRequired();
            #endregion


            #region Place
            modelBuilder.Entity<Place>()
                .HasKey(e => e.Id);

            //realcje
            modelBuilder.Entity<Place>()
                .HasOne(e => e.PlaceAvailabilityTime)
                .WithOne()
                .HasForeignKey<PlaceAvailabilityTime>(e => e.PlaceId)
                .OnDelete(DeleteBehavior.NoAction)
                .IsRequired();

            modelBuilder.Entity<Place>()
                .HasMany(e => e.Comments)
                .WithOne()
                .HasForeignKey(e => e.PlaceId)
                .OnDelete(DeleteBehavior.NoAction)
                .IsRequired();

            modelBuilder.Entity<Place>()
                .HasMany(e => e.TripsThatIncludeThisPlace)
                .WithOne(e => e.Place)
                .HasForeignKey(e => e.PlaceId)
                .OnDelete(DeleteBehavior.NoAction)
                .IsRequired();
            //

            modelBuilder.Entity<Place>()
                .Property(p => p.Id)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<Place>()
                .Property(s => s.Name)
                .IsRequired();

            modelBuilder.Entity<Place>()
               .Property(s => s.Coordinates)
               .IsRequired();

            modelBuilder.Entity<Place>()
               .Property(s => s.Description);

            modelBuilder.Entity<Place>()
               .Property(s => s.EstimatedCost);

            modelBuilder.Entity<Place>()
               .Property(s => s.Address);

            modelBuilder.Entity<Place>()
                .Property(s => s.AverageTimeSpent);

            modelBuilder.Entity<PlaceAvailabilityTime>()
                .HasKey(e => e.Id);
            #endregion


            #region Trip
            modelBuilder.Entity<Trip>()
            .HasKey(e => e.Id);

            //realcje
            modelBuilder.Entity<Trip>()
                .HasMany(e => e.Places)
                .WithOne(e => e.Trip)
                .HasForeignKey(e => e.TripId)
                .OnDelete(DeleteBehavior.NoAction)
                .IsRequired();
            //

            modelBuilder.Entity<Trip>()
                .Property(p => p.Id)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<Trip>()
                .Property(s => s.Title)
                .IsRequired();

            modelBuilder.Entity<Trip>()
               .Property(s => s.Description)
               .IsRequired();

            modelBuilder.Entity<Trip>()
               .Property(s => s.Author);

            modelBuilder.Entity<Trip>()
               .Property(s => s.StartHour);

            modelBuilder.Entity<Trip>()
               .Property(s => s.StopHour);

            modelBuilder.Entity<Trip>()
                .Property(s => s.Date);

            modelBuilder.Entity<Trip>()
                .Property(s => s.TransportType);

            modelBuilder.Entity<Trip>()
                .Property(s => s.Budget);

            modelBuilder.Entity<Trip>()
                .Property(s => s.IsPublic);
            #endregion
        }
    }
}
