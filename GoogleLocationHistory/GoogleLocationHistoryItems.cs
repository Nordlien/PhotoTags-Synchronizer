using System;
using System.Collections.Generic;
using System.Linq;


namespace GoogleLocationHistory
{

    public class GoogleLocationHistoryItems
    {
        private String googleTakeoutAccount;
        private SortedList<DateTime, GoogleJsonLocations> googleLocationItems;

        public GoogleLocationHistoryItems(string userAccount)
        {
            googleTakeoutAccount = userAccount;
            googleLocationItems = new SortedList<DateTime, GoogleJsonLocations>();
        }

        public void Add(GoogleJsonLocations googleJsonLocationsItem)
        {
            googleLocationItems.Add(googleJsonLocationsItem.Timestamp, googleJsonLocationsItem);
        }
    }
}
