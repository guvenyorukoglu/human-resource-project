using System.ComponentModel.DataAnnotations;

namespace humanResourceProject.Domain.Enum
{
    public enum Currency
    {
        [Display(Name = "Türk Lirası(₺)")]
        TRY,
        [Display(Name = "Dolar($)")]
        USD,
        [Display(Name = "Euro(€)")]
        EUR,
        [Display(Name = "Pound(£)")]
        GBP,
        [Display(Name = "İsviçre Frangı(CHF)")]
        CHF,
        [Display(Name = "Japon Yeni(¥)")]
        JPY,
        [Display(Name = "Avustralya Doları(AUD)")]
        AUD,
        [Display(Name = "Kanada Doları(CAD)")]
        CAD,
        [Display(Name = "Çin Yuanı(¥)")]
        CNH,
        [Display(Name = "Hong Kong Doları(HKD)")]
        HKD,
        [Display(Name = "Yeni Zelanda Doları(NZD)")]
        NZD
    }
}
