#
// File ini berisi TODO -- lihat SOAL.md untuk kontrak lengkap tiap level.
// Bagian bertanda TODO(Level N) adalah tugas kalian; kode lain sudah
// disediakan. Jangan mengubah nama/tipe yang sudah ada kecuali TODO
// memintanya secara eksplisit.

using System;
using System.Linq;

namespace Pertemuan04;

public class Buku
{
    // Level 1 : Field private //
    private string _isbn = "";
    private string _judul = "";
    private int _stokTotal;
    private int _stokTersedia;

    // Level 1 : Properti read-only //    
    public string Isbn => _isbn;
    public string Judul => _judul;
    public int StokTotal => _stokTotal;
    public int StokTersedia => _stokTersedia;

    // Level 8: properti dengan validasi di accessor set
    private int _batasHariPinjam = 7;
    public int BatasHariPinjam
    {
        get => _batasHariPinjam;
        set
        {
            if (value < 1 || value > 30)
            {
                throw new ArgumentOutOfRangeException(nameof(value), "Batas hari pinjam harus antara 1 dan 30.");
            }
            _batasHariPinjam = value;
        }
    }

    // Level 2 & 6: Konstruktor dengan validasi parameter dan normalisasi ISBN-13
    public Buku(string isbn, string judul, int stokTotal)
    {
        // Level 2: Validasi judul (null, kosong, atau spasi saja)
        if (string.IsNullOrWhiteSpace(judul))
        {
            throw new ArgumentException("Judul tidak boleh kosong atau hanya berisi spasi.", nameof(judul));
        }

        // Level 2: Validasi stokTotal (negatif)
        if (stokTotal < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(stokTotal), "Stok total tidak boleh negatif.");
        }

        // Level 6: Normalisasi dan Validasi ISBN-13
        _isbn = NormalisasiDanValidasiIsbn(isbn);
        _judul = judul;
        _stokTotal = stokTotal;
        _stokTersedia = stokTotal; // Level 1: StokTersedia awal sama dengan stokTotal
    }

    public void Pinjam()
    {
        // Level 3: kurangi StokTersedia satu. Kalau stok sudah 0, lempar InvalidOperationException
        if (_stokTersedia <= 0)
        {
            throw new InvalidOperationException("Stok buku sudah habis, tidak dapat dipinjam.");
        }
        _stokTersedia--;
    }

    public void Kembalikan()
    {
        // Level 4: tambah StokTersedia satu. Kalau stok sudah sama dengan StokTotal, lempar InvalidOperationException
        if (_stokTersedia >= _stokTotal)
        {
            throw new InvalidOperationException("Semua eksemplar buku sudah berada di perpustakaan.");
        }
        _stokTersedia++;
    }

    // Level 5: properti TERHITUNG -- tanpa field pendukung, tanpa setter.
    public double PersentaseTersedia
    {
        get
        {
            // Level 5: kembalikan StokTersedia / StokTotal * 100 (double). Kalau StokTotal = 0 kembalikan 0.
            if (_stokTotal == 0) return 0;
            return (double)_stokTersedia / _stokTotal * 100.0;
        }
    }

    public string Status
    {
        get
        {
            // Level 5: kembalikan "Tersedia" kalau StokTersedia > 0, selain itu "Habis".
            return _stokTersedia > 0 ? "Tersedia" : "Habis";
        }
    }

    // Helper untuk Level 6: Normalisasi dan Validasi ISBN-13
    private static string NormalisasiDanValidasiIsbn(string isbnInput)
    {
        if (string.IsNullOrWhiteSpace(isbnInput))
        {
            throw new ArgumentException("ISBN tidak boleh null atau kosong.", nameof(isbnInput));
        }

        // Buang semua tanda '-' dan spasi
        string bersih = isbnInput.Replace("-", "").Replace(" ", "");

        // Harus tepat 13 digit angka
        if (bersih.Length != 13 || !bersih.All(char.IsDigit))
        {
            throw new ArgumentException("ISBN harus terdiri dari tepat 13 digit angka setelah dibersihkan.", nameof(isbnInput));
        }

        // Validasi digit cek ISBN-13 (bobot berselang-seling 1, 3, 1, 3, ...)
        int sum = 0;
        for (int i = 0; i < 13; i++)
        {
            int digit = bersih[i] - '0';
            int bobot = (i % 2 == 0) ? 1 : 3;
            sum += digit * bobot;
        }

        if (sum % 10 != 0)
        {
            throw new ArgumentException("Digit cek ISBN-13 tidak valid.", nameof(isbnInput));
        }

        return bersih;
    }
}
