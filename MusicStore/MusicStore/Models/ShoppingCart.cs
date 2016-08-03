using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MusicStore.Models
{
    public class ShoppingCart
    {
        /*
        public static int GetCartCount()
        {
            using (MusicStoreEntity db = new MusicStoreEntity())
            {
                int? count = db.Carts.Where(p => p.CartId == HttpContext.Current.User.Identity.Name)
                    .Select(p => (int?)p.Count).Sum();

                return count ?? 0;
            }
        }
        */
        public static int CartCount
        {
            get { 
            using (MusicStoreEntity db = new MusicStoreEntity())
            {
                int? count = db.Carts.Where(p => p.CartId == HttpContext.Current.User.Identity.Name)
                    .Select(p => (int?)p.Count).Sum();

                return count ?? 0;
            }
            }
        }

        public static decimal CartTotal
        {
            get
            {
                using (MusicStoreEntity db = new MusicStoreEntity())
                {
                    decimal? count = db.Carts.Where(p => p.CartId == HttpContext.Current.User.Identity.Name)
                        .Select(p => (int?)p.Count*p.Albums.Price).Sum();

                    return count ?? 0;
                }
            }
        }
    }
}