using EntityLayer;

namespace StokTakipUI
{
    public static class CurrentSession
    {
        // Giriş yapan kullanıcıyı program kapanana kadar burada saklayacağız
        public static User CurrentUser { get; set; }
    }
}