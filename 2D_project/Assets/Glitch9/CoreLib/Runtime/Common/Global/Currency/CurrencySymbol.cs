namespace Glitch9
{
    public static class CurrencySymbol
    {
        public static string GetSymbol(CurrencyCode currencyCode)
        {
            return currencyCode switch
            {
                CurrencyCode.USD => "$",
                CurrencyCode.EUR => "€",
                CurrencyCode.GBP => "£",
                CurrencyCode.JPY => "¥",
                CurrencyCode.CAD => "C$",
                CurrencyCode.AUD => "A$",
                CurrencyCode.CHF => "CHF",
                CurrencyCode.CNY => "¥",
                CurrencyCode.HKD => "HK$",
                CurrencyCode.SGD => "S$",
                CurrencyCode.NZD => "NZ$",
                CurrencyCode.KRW => "₩",
                CurrencyCode.INR => "₹",
                CurrencyCode.RUB => "₽",
                CurrencyCode.BRL => "R$",
                CurrencyCode.ZAR => "R",
                CurrencyCode.SEK => "kr",
                CurrencyCode.NOK => "kr",
                CurrencyCode.DKK => "kr",
                CurrencyCode.PLN => "zł",
                CurrencyCode.TRY => "₺",
                CurrencyCode.THB => "฿",
                CurrencyCode.MYR => "RM",
                CurrencyCode.IDR => "Rp",
                CurrencyCode.PHP => "₱",
                CurrencyCode.VND => "₫",
                _ => "?"
            };
        }
    }
}
