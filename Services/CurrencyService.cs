using Blazored.LocalStorage;

namespace UttamCalculatorsBlazor.Services;

public enum CurrencyCode
{
    INR, // Indian Rupee
    USD, // US Dollar
    EUR, // Euro
    GBP, // British Pound
    AUD, // Australian Dollar
    CAD, // Canadian Dollar
    JPY, // Japanese Yen
    CNY, // Chinese Yuan
    CHF, // Swiss Franc
    RUB, // Russian Ruble
    BRL, // Brazilian Real
    ZAR, // South African Rand
    NZD, // New Zealand Dollar
    SGD, // Singapore Dollar
    MXN, // Mexican Peso
    KRW, // South Korean Won
    MYR, // Malaysian Ringgit
    HKD, // Hong Kong Dollar
    SEK, // Swedish Krona
    NOK, // Norwegian Krone
}

public class CurrencyService
{
    private readonly ILocalStorageService _localStorage;
    private CurrencyCode _cachedCurrency;

    public CurrencyService(ILocalStorageService localStorage)
    {
        _localStorage = localStorage;
    }

    // Initialize and cache the selected currency
    public async Task InitializeAsync()
    {
        var storedCurrency = await _localStorage.GetItemAsync<string>("selectedCurrency");
        if (!string.IsNullOrEmpty(storedCurrency) && Enum.TryParse<CurrencyCode>(storedCurrency, out var currencyCode))
        {
            _cachedCurrency = currencyCode;
        }
        else
        {
            // Default to INR if nothing is found in local storage
            _cachedCurrency = CurrencyCode.INR;
        }
    }

    // Update the cached currency when it changes
    public async Task SetCurrency(CurrencyCode currency)
    {
        _cachedCurrency = currency;

        // Store the selected currency in local storage
        await _localStorage.SetItemAsync("selectedCurrency", currency.ToString());
    }

    // Get the cached currency
    public CurrencyCode GetCachedCurrency()
    {
        return _cachedCurrency;
    }


    public string ConvertToCurrencyFormat(double? number, bool? showSymbol = true)
    {
        string currencySymbol = _cachedCurrency switch
        {
            CurrencyCode.INR => "₹",   // Indian Rupee
            CurrencyCode.USD => "$",   // US Dollar
            CurrencyCode.EUR => "€",   // Euro
            CurrencyCode.GBP => "£",   // British Pound
                                       // Add more currencies here
            _ => "₹" // Default to INR symbol if no match
        };

        var cultureInfo = _cachedCurrency switch
        {
            CurrencyCode.INR => new System.Globalization.CultureInfo("en-IN"),
            CurrencyCode.USD => new System.Globalization.CultureInfo("en-US"),
            CurrencyCode.EUR => new System.Globalization.CultureInfo("fr-FR"),
            CurrencyCode.GBP => new System.Globalization.CultureInfo("en-GB"),
            // Add more currencies here
            _ => new System.Globalization.CultureInfo("en-IN") // Default to INR
        };

        string formattedNumber = string.Format(cultureInfo, "{0:N2}", number ?? 0);

        // If showSymbol is true or not explicitly set to false, show the currency symbol
        return showSymbol == true ? $"{currencySymbol}{formattedNumber}" : formattedNumber;
    }


}
