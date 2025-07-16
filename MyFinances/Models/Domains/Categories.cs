namespace MyFinances.Models.Domains
{
    public static class Categories
    {
        public static readonly List<LookupItem> Items =
        [
            new LookupItem { Id = 1, Name = "Ogólna" },
            new LookupItem { Id = 2, Name = "Media i usługi" },
            new LookupItem { Id = 3, Name = "Nieruchomości" },
            new LookupItem { Id = 4, Name = "Wyżywienie" },
            new LookupItem { Id = 5, Name = "Rozrywka" },
            new LookupItem { Id = 6, Name = "Ubrania" },
            new LookupItem { Id = 7, Name = "Transport" },
            new LookupItem { Id = 8, Name = "Edukacja" },
            new LookupItem { Id = 9, Name = "Podatki" },
            new LookupItem { Id = 10, Name = "Zwierzęta" },
            new LookupItem { Id = 11, Name = "Darowizny" },
            new LookupItem { Id = 12, Name = "Zdrowie" },
            new LookupItem { Id = 13, Name = "Inwestycje" },
            new LookupItem { Id = 14, Name = "Kary" }
        ];
    }
}
