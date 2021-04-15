# Genel Bilgi
Evimize devamlı bir şeyler alıyoruz. Bazen bir dolap alıyoruz ve sonra ne zaman aldığımızı görmek istiyoruz. Bazen ise en son ne zaman çamaşır deterjanı aldık da hemen yine bitti diye düşünüyoruz. Bu projeye başlama motivasyonum, evdeki eşyaların ve tüketim malzemelerinin takibini kolaylaştırmak; ve devamında da deterjan ortalama 1 ay gidiyorsa ve 1 aydır yenisini almadıysam bildirim ile bana hatırlatacak bir sistem hazırlamak.

# Ortam Kurulumu
Ortam kurulumu son derece basittir. Kodu bilgisayarınıza alıp Visual Studio ile açarsanız, derleme sırasında Nuget üzerinden gerekli paketler indirilir.

Veritabanı olarak SqlServer kullandığım için bilgisayarınızda SqlServer kurulu olmalıdır. Eğer farklı bir veritabanı kullanacaksanız, istediğiniz bir veritabanı ile değiştirebilirsiniz. Startup.cs içerisinde gerekli değişikliği yapmanız gerekir. Ayrıca, eğer Entity Framework Identity tarafından desteklenmeyen bir veritabanı kullanacaksanız, Authentication ve Authorization işlemlerini kendiniz yazmanız gerekir.

# Derleme ve Testler
Derleme için ek bir ihtiyaç bulunmamaktadır. Visual Studio içinden derleyebilirsiniz.

Testler için ise, yine Visual Studio'nun Test Explorer penceresini kullanarak tüm testleri koşturabilirsiniz.

# Destek
Bu uygulamanın geliştirmesine katkıda bulunmak isterseniz teknik konularla ilgili yardımcı olabilirim. Yapmanız gereken: kodu kendi kullanıcı hesabınıza Forklamak ve geliştirmelerinizi Pull Request ile göndermek. 

Issues bölümünü mümkün olduğunca aktif tutuyorum. Oradaki "Help Wanted" veya "Good First Issue" olarak etiketlenmiş maddelere göz atabilirsiniz.