using JuggleWebApp.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace JuggleWebApp
{
    public static class Converters
    {
        public static List<SelectListItem> TournamentListToItemListConverter(List<Tournament> tournaments)
        {
            List<SelectListItem> selectListItems = new List<SelectListItem>();

            for (int i = 0; i < tournaments.Count(); i++)
            {
                selectListItems.Add(new SelectListItem()
                {
                    Value = $"{i+1}",
                    Text = tournaments[i].Title,
                });
            }
            return selectListItems;
        }
    }
}
