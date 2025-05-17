--VIEWS
USE Museum;

--Eserler, sanatçý ve tür bilgilerini birleþtirir.
CREATE VIEW vw_EserDetaylari AS
SELECT 
    e.ID AS EserID,
    e.Ad AS EserAdi,
    s.Ad AS SanatciAdi,
    t.Ad AS EserTuru,
    e.YapimYili,
    e.BulunduguMuze,
    e.MevcutDurum
FROM Eserler e
INNER JOIN Sanatcilar s ON e.Sanatci_ID = s.ID
INNER JOIN EserTurleri t ON e.Tur_ID = t.ID;

SELECT * FROM vw_EserDetaylari;


--Sadece þu anda aktif olan sergileri listeler.
CREATE VIEW vw_AktifSergiler AS
SELECT 
    ID,
    Ad,
    Konum,
    BaslangicTarihi,
    BitisTarihi
FROM Sergiler
WHERE GETDATE() BETWEEN BaslangicTarihi AND BitisTarihi;

SELECT * FROM vw_AktifSergiler;


--Ziyaretçilerin katýldýðý etkinlikleri gösterir.   
CREATE VIEW vw_ZiyaretciEtkinlikKayitlari AS
SELECT 
    z.Ad + ' ' + z.Soyad AS ZiyaretciAdSoyad,
    e.Ad AS EtkinlikAdi,
    ek.KayitTarihi
FROM EtkinlikKayitlari ek
INNER JOIN Ziyaretciler z ON ek.ZiyaretciID = z.ID
INNER JOIN Etkinlikler e ON ek.EtkinlikID = e.ID;


SELECT * FROM vw_ZiyaretciEtkinlikKayitlari;


--Müze gelir ve giderlerini özetler.    
CREATE VIEW vw_GelirGiderOzet AS
SELECT 'Gelir' AS Tip, SUM(Tutar) AS ToplamTutar FROM MuzeGelirleri
UNION
SELECT 'Gider' AS Tip, SUM(Tutar) FROM MuzeGiderleri;

SELECT * FROM vw_GelirGiderOzet;


--Her eserin bakým geçmiþini listeler.  
CREATE VIEW vw_EserBakimGecmisi AS
SELECT 
    e.Ad AS EserAdi,
    b.BakimTarihi,
    b.YapilanIslem,
    p.Ad + ' ' + p.Soyad AS SorumluPersonel
FROM EserBakimKayitlari b
INNER JOIN Eserler e ON b.EserID = e.ID
INNER JOIN Personel p ON b.SorumluKisi = p.ID;

SELECT * FROM vw_EserBakimGecmisi;



--Sanatçýlarýn dahil olduðu sanat akýmlarýný listeler.  
CREATE VIEW vw_SanatciAkimlari AS
SELECT 
    s.Ad AS Sanatci,
    a.Ad AS Akim,
    a.Aciklama
FROM SanatciAkim sa
JOIN Sanatcilar s ON sa.SanatciID = s.ID
JOIN SanatAkimlari a ON sa.AkimID = a.ID;

SELECT * FROM vw_SanatciAkimlari;


--Yalnýzca üyelikli ziyaretçileri listeler.  
CREATE VIEW vw_UyelikliZiyaretciler AS
SELECT 
    Ad,
    Soyad,
	DogumTarihi,
    Email
FROM Ziyaretciler
WHERE UyelikDurumu = 1;

SELECT * FROM vw_UyelikliZiyaretciler;


--Tüm eser transfer geçmiþini özetler.
CREATE VIEW vw_EserTransferKayitlari AS
SELECT 
    e.Ad AS EserAdi,
    t.KaynakMuze,
    t.HedefMuze,
    t.Tarih,
    t.TransferDurumu
FROM EserTransferleri t
INNER JOIN Eserler e ON t.EserID = e.ID;

SELECT * FROM vw_EserTransferKayitlari;


--Ziyaretçi giriþ-çýkýþ bilgilerini sergi ve etkinlik ile birlikte gösterir.  
CREATE VIEW vw_ZiyaretciGirisRaporu AS
SELECT 
    z.Ad + ' ' + z.Soyad AS Ziyaretci,
    g.GirisTarihi,
    g.CikisTarihi,
    s.Ad AS Sergi,
    e.Ad AS Etkinlik
FROM ZiyaretciGirisKayitlari g
INNER JOIN Ziyaretciler z ON g.ZiyaretciID = z.ID
LEFT JOIN Sergiler s ON g.SergiID = s.ID
LEFT JOIN Etkinlikler e ON g.EtkinlikID = e.ID;

SELECT * FROM vw_ZiyaretciGirisRaporu;


--En çok katýlým alan etkinlikleri gösterir.  DESC DÜZELT
CREATE VIEW vw_PopulerEtkinlikler AS
SELECT 
    e.Ad AS EtkinlikAdi,
    COUNT(ek.ID) AS KatilimSayisi
FROM EtkinlikKayitlari ek
INNER JOIN Etkinlikler e ON ek.EtkinlikID = e.ID
GROUP BY e.Ad
ORDER BY KatilimSayisi DESC;

SELECT * FROM vw_ZiyaretciGirisRaporu;


CREATE VIEW vw_MuzeGiderOzet AS
SELECT 
    YEAR(Tarih) AS Yil,
    MONTH(Tarih) AS Ay,
    COUNT(*) AS GiderSayisi,
    SUM(Tutar) AS ToplamGider,
    STRING_AGG(Aciklama, '; ') AS GiderAciklamalari
FROM MuzeGiderleri
WHERE Tarih IS NOT NULL
GROUP BY YEAR(Tarih), MONTH(Tarih);

SELECT * FROM vw_MuzeGiderOzet;


CREATE VIEW vw_SanatcilarinAkimlari AS
SELECT 
    s.Ad AS Sanatci,
    a.Ad AS Akim,
    a.Aciklama
FROM SanatciAkim sa
JOIN Sanatcilar s ON sa.SanatciID = s.ID
JOIN SanatAkimlari a ON sa.AkimID = a.ID;

SELECT * FROM vw_SanatcilarinAkimlari;











--Her Eserin Kaç Sergide Yer Aldýðý
CREATE VIEW vw_EserSergiSayilari AS
SELECT E.ID AS EserID, E.Ad AS EserAdi, COUNT(ES.ID) AS SergiSayisi
FROM Eserler E
LEFT JOIN EserSergileri ES ON E.ID = ES.EserID
GROUP BY E.ID, E.Ad;

SELECT * FROM vw_EserSergiSayilari;

--**Ziyaretçi Giriþ Çýkýþ Günlük Log (buna tekrar bak)
CREATE VIEW GunlukZiyaretciLog AS
SELECT Z.Ad + ' ' + Z.Soyad AS ZiyaretciAdi, G.GirisTarihi, G.CikisTarihi, B.Ad AS BiletTuru
FROM ZiyaretciGirisKayitlari G
JOIN Ziyaretciler Z ON G.ZiyaretciID = Z.ID
JOIN BiletTurleri B ON G.BiletTuru = B.ID;


--Ziyaretçi bilgileri ve giriþ kayýtlarý (buna tekrar bak)
CREATE VIEW ZiyaretciGirisBilgisi AS
SELECT z.Ad, z.Soyad, z.Email, g.GirisTarihi, g.CikisTarihi, b.Ad AS BiletTuru
FROM Ziyaretciler z
JOIN ZiyaretciGirisKayitlari g ON z.ID = g.ZiyaretciID
JOIN BiletTurleri b ON g.BiletTuru = b.ID;


--Bakým gerektiren eserler (son bakým > 2 yýl) BUNA TEKRAR BAK
CREATE VIEW BakimGerekenEserler AS
SELECT e.Ad, MAX(b.BakimTarihi) AS SonBakimTarihi
FROM Eserler e
JOIN EserBakimKayitlari b ON e.ID = b.EserID
GROUP BY e.Ad
HAVING MAX(b.BakimTarihi) < DATEADD(YEAR, -2, GETDATE());


--Son etkinlik kayýtlarý
CREATE VIEW SonEtkinlikKayitlari AS
SELECT z.Ad + ' ' + z.Soyad AS ZiyaretciAdi, e.Ad AS EtkinlikAdi, k.KayitTarihi
FROM EtkinlikKayitlari k
JOIN Ziyaretciler z ON k.ZiyaretciID = z.ID
JOIN Etkinlikler e ON k.EtkinlikID = e.ID
WHERE k.KayitTarihi > DATEADD(MONTH, -1, GETDATE());


--Baðýþçý detaylarý
CREATE VIEW vw_BagisciDetaylari AS
SELECT b.Ad + ' ' + b.Soyad AS BagisciAdi, d.Miktar, d.BagisTarihi, d.KullanimAlani
FROM Bagiscilar b
JOIN Bagislar d ON b.ID = d.BagisciID;

SELECT * FROM vw_BagisciDetaylari;


--Personel maaþ listesi
CREATE VIEW vw_PersonelMaasListesi AS
SELECT Ad + ' ' + Soyad AS AdSoyad, Gorev, Maas
FROM Personel;

SELECT * FROM vw_PersonelMaasListesi;


--Sergideki eser sayýsý
CREATE VIEW SergiEserSayisi AS
SELECT s.Ad, COUNT(es.EserID) AS EserSayisi
FROM Sergiler s
JOIN EserSergileri es ON s.ID = es.SergiID
GROUP BY s.Ad;






--Stored Procedures




--yeni eser ekleme
CREATE PROCEDURE sp_EserEkleme
    @Ad NVARCHAR(100),
    @TurID INT,
    @SanatciID INT,
    @YapimYili INT,
    @BulunduguMuze NVARCHAR(100),
    @MevcutDurum NVARCHAR(50),
    @DijitalKoleksiyonURL NVARCHAR(255)
AS
BEGIN
    INSERT INTO Eserler (Ad, Tur_ID, Sanatci_ID, YapimYili, BulunduguMuze, MevcutDurum, DijitalKoleksiyonURL)
    VALUES (@Ad, @TurID, @SanatciID, @YapimYili, @BulunduguMuze, @MevcutDurum, @DijitalKoleksiyonURL);
END

EXEC sp_EserEkleme
    @Ad = N'Modern Heykel',
    @TurID = 2,
    @SanatciID = 1,
    @YapimYili = 2010,
    @BulunduguMuze = N'Modern Sanatlar Müzesi',
    @MevcutDurum = N'Sergileniyor',
    @DijitalKoleksiyonURL = N'https://ornekurl.com/heykel1';


SELECT * FROM Eserler;




--eser bakýmý kaydetme
CREATE PROCEDURE sp_EserBakimiEkle
    @EserID INT,
    @BakimTarihi DATE,
    @YapilanIslem NVARCHAR(255),
    @SorumluKisi INT
AS
BEGIN
    INSERT INTO EserBakimKayitlari (EserID, BakimTarihi, YapilanIslem, SorumluKisi)
    VALUES (@EserID, @BakimTarihi, @YapilanIslem, @SorumluKisi);
END

EXEC sp_EserBakimiEkle
    @EserID = 1,
    @BakimTarihi = '2024-01-15',
    @YapilanIslem = N'Temizlik ve koruma cilasý',
    @SorumluKisi = 3;

SELECT * FROM EserBakimKayitlari;




--sanatçýya sanat akýmý ekleme
CREATE PROCEDURE sp_SanatciyaAkimEkle
    @SanatciID INT,
    @AkimID INT
AS
BEGIN
    IF NOT EXISTS (SELECT * FROM SanatciAkim WHERE SanatciID = @SanatciID AND AkimID = @AkimID)
    BEGIN
        INSERT INTO SanatciAkim (SanatciID, AkimID)
        VALUES (@SanatciID, @AkimID);
    END
END

EXEC sp_SanatciyaAkimEkle
    @SanatciID = 1,
    @AkimID = 2;

SELECT * FROM SanatciAKim;


--sergihye eser ekelme
CREATE PROCEDURE sp_EseriSergiyeEkle
    @EserID INT,
    @SergiID INT,
    @BaslangicTarihi DATE,
    @BitisTarihi DATE
AS
BEGIN
    INSERT INTO EserSergileri (EserID, SergiID, BaslangicTarihi, BitisTarihi)
    VALUES (@EserID, @SergiID, @BaslangicTarihi, @BitisTarihi);
END


--ziyaretçi kaydý ekleme
CREATE PROCEDURE sp_ZiyaretciEkle
    @Ad NVARCHAR(50),
    @Soyad NVARCHAR(50),
    @DogumTarihi DATE,
    @Email NVARCHAR(100),
    @UyelikDurumu BIT
AS
BEGIN
    INSERT INTO Ziyaretciler (Ad, Soyad, DogumTarihi, Email, UyelikDurumu)
    VALUES (@Ad, @Soyad, @DogumTarihi, @Email, @UyelikDurumu);
END


--ziyaretçi giriþ kaydý eklme
CREATE PROCEDURE sp_ZiyaretciGirisEkle
    @ZiyaretciID INT,
    @GirisTarihi DATETIME,
    @CikisTarihi DATETIME,
    @BiletTuru INT,
    @SergiID INT = NULL,
    @EtkinlikID INT = NULL
AS
BEGIN
    INSERT INTO ZiyaretciGirisKayitlari (ZiyaretciID, GirisTarihi, CikisTarihi, BiletTuru, SergiID, EtkinlikID)
    VALUES (@ZiyaretciID, @GirisTarihi, @CikisTarihi, @BiletTuru, @SergiID, @EtkinlikID);
END



--etkinlik kaydý ekleme
CREATE PROCEDURE sp_EtkinlikEkle
    @Ad NVARCHAR(100),
    @Tur NVARCHAR(50),
    @BaslangicTarihi DATE,
    @BitisTarihi DATE,
    @Aciklama NVARCHAR(255)
AS
BEGIN
    INSERT INTO Etkinlikler (Ad, Tur, BaslangicTarihi, BitisTarihi, Aciklama)
    VALUES (@Ad, @Tur, @BaslangicTarihi, @BitisTarihi, @Aciklama);
END



--eser transferi kaydetme
CREATE PROCEDURE sp_EserTransferEkle
    @EserID INT,
    @KaynakMuze NVARCHAR(100),
    @HedefMuze NVARCHAR(100),
    @Tarih DATE,
    @TransferDurumu NVARCHAR(50)
AS
BEGIN
    INSERT INTO EserTransferleri (EserID, KaynakMuze, HedefMuze, Tarih, TransferDurumu)
    VALUES (@EserID, @KaynakMuze, @HedefMuze, @Tarih, @TransferDurumu);
END



--gelir kaydý ekleme
CREATE PROCEDURE sp_MuzeGelirEkle
    @KaynakTuru NVARCHAR(50),
    @Tutar DECIMAL(10,2),
    @Tarih DATE
AS
BEGIN
    INSERT INTO MuzeGelirleri (KaynakTuru, Tutar, Tarih)
    VALUES (@KaynakTuru, @Tutar, @Tarih);
END



--gider kaydý eklme
CREATE PROCEDURE sp_MuzeGiderEkle
    @Aciklama NVARCHAR(255),
    @Tutar DECIMAL(10,2),
    @Tarih DATE
AS
BEGIN
    INSERT INTO MuzeGiderleri (Aciklama, Tutar, Tarih)
    VALUES (@Aciklama, @Tutar, @Tarih);
END

