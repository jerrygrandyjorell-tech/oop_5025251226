// File ini disediakan dosen untuk mengecek progres level secara otomatis.
// JANGAN DIUBAH — perubahan pada file ini tidak akan dipakai saat penilaian
// (dosen menimpa ulang folder tests/ sebelum menjalankan grading).
using System.Reflection;
using System.Runtime.CompilerServices;
using Pertemuan05;
using Xunit;

namespace Pertemuan05.Tests;

public class PewarisanTests
{
    // ---------- helper ----------

    private static Alamat AlamatContoh() => new("Jl. Mawar 5", "Surabaya");

    private static Mahasiswa BuatMahasiswa(string id = "M01", string nama = "Sari") =>
        new(id, nama, AlamatContoh(), "5025201001", "Informatika");

    private static Dosen BuatDosen(string id = "D01", string nama = "Pak Budi") =>
        new(id, nama, AlamatContoh(), "198001012005011001");

    // Membaca berkas di folder pertemuan (satu tingkat di atas tests/).
    private static string BacaFileDiFolderPertemuan(
        string namaFile,
        [CallerFilePath] string testFilePath = "")
    {
        var direktoriTests = Path.GetDirectoryName(testFilePath)!;
        var direktoriPertemuan = Directory.GetParent(direktoriTests)!.FullName;
        var target = Path.Combine(direktoriPertemuan, namaFile);
        return File.Exists(target) ? File.ReadAllText(target) : "";
    }

    // ---------- Level 1 ----------

    [Fact(DisplayName = "Level 1 - Komposisi: kelas Alamat")]
    public void Level1_Alamat()
    {
        var alamat = new Alamat("Jl. Mawar 5", "Surabaya");
        Assert.Equal("Jl. Mawar 5", alamat.Jalan);
        Assert.Equal("Surabaya", alamat.Kota);
        Assert.Equal("Jl. Mawar 5, Surabaya", alamat.ToString());

        Assert.ThrowsAny<ArgumentException>(() => new Alamat("", "Surabaya"));
        Assert.ThrowsAny<ArgumentException>(() => new Alamat("Jl. Mawar 5", "   "));
        Assert.ThrowsAny<ArgumentException>(() => new Alamat(null!, "Surabaya"));
    }

    // ---------- Level 2 ----------

    [Fact(DisplayName = "Level 2 - Anggota memiliki Alamat (has-a)")]
    public void Level2_AnggotaMemilikiAlamat()
    {
        var alamat = AlamatContoh();
        var anggota = new Anggota("A01", "Budi", alamat);

        Assert.Equal("A01", anggota.Id);
        Assert.Equal("Budi", anggota.Nama);
        Assert.Same(alamat, anggota.Alamat);
        Assert.Equal("A01 - Budi", anggota.Info());
        Assert.Equal(2, anggota.BatasPinjam);

        Assert.ThrowsAny<ArgumentException>(() => new Anggota("", "Budi", alamat));
        Assert.ThrowsAny<ArgumentException>(() => new Anggota("A01", "  ", alamat));
        Assert.Throws<ArgumentNullException>(() => new Anggota("A01", "Budi", null!));
    }

    // ---------- Level 3 ----------

    [Fact(DisplayName = "Level 3 - Pewarisan: Mahasiswa & Dosen (rantai konstruktor)")]
    public void Level3_PewarisanKonstruktor()
    {
        Assert.Equal(typeof(Anggota), typeof(Mahasiswa).BaseType);
        Assert.Equal(typeof(Anggota), typeof(Dosen).BaseType);

        // Mahasiswa dipakai lewat variabel bertipe induk (upcasting)
        Anggota a = BuatMahasiswa("M01", "Sari");
        Assert.Equal("M01", a.Id);          // diisi oleh konstruktor kelas induk
        Assert.Equal("M01 - Sari", a.Info());
        var m = (Mahasiswa)a;
        Assert.Equal("5025201001", m.Nrp);
        Assert.Equal("Informatika", m.Prodi);

        Anggota b = BuatDosen("D01", "Pak Budi");
        Assert.Equal("D01 - Pak Budi", b.Info());
        Assert.Equal("198001012005011001", ((Dosen)b).Nip);

        Assert.ThrowsAny<ArgumentException>(() => new Mahasiswa("M01", "Sari", AlamatContoh(), "", "Informatika"));
        Assert.ThrowsAny<ArgumentException>(() => new Mahasiswa("M01", "Sari", AlamatContoh(), "5025201001", " "));
        Assert.ThrowsAny<ArgumentException>(() => new Dosen("D01", "Pak Budi", AlamatContoh(), ""));
    }

    // ---------- Level 4 ----------

    [Fact(DisplayName = "Level 4 - Modifier protected: BatasPinjam")]
    public void Level4_ProtectedBatasPinjam()
    {
        var prop = typeof(Anggota).GetProperty("BatasPinjam", BindingFlags.Public | BindingFlags.Instance);
        Assert.True(prop is not null, "Properti publik BatasPinjam tidak ditemukan di Anggota");
        Assert.Null(prop!.GetSetMethod(nonPublic: false));
        var setter = prop.GetSetMethod(nonPublic: true);
        Assert.True(setter is not null && setter.IsFamily,
            "Setter BatasPinjam harus `protected` (bukan public, bukan private)");

        Assert.Equal(2, new Anggota("A01", "Budi", AlamatContoh()).BatasPinjam);
        Assert.Equal(3, BuatMahasiswa().BatasPinjam);
        Assert.Equal(10, BuatDosen().BatasPinjam);
    }

    // ---------- Level 5 ----------

    [Fact(DisplayName = "Level 5 - Pinjam() memakai BatasPinjam milik tiap jenis")]
    public void Level5_PinjamMemakaiBatas()
    {
        Anggota m = BuatMahasiswa();
        Assert.Equal(0, m.JumlahPinjam);
        m.Pinjam("Buku 1");
        m.Pinjam("Buku 2");
        m.Pinjam("Buku 3");
        Assert.Equal(3, m.JumlahPinjam);
        Assert.Throws<InvalidOperationException>(() => m.Pinjam("Buku 4"));
        Assert.Equal(3, m.JumlahPinjam);

        Anggota d = BuatDosen();
        for (int i = 1; i <= 10; i++) d.Pinjam($"Buku {i}");
        Assert.Equal(10, d.JumlahPinjam);
        Assert.Throws<InvalidOperationException>(() => d.Pinjam("Buku 11"));

        var biasa = new Anggota("A01", "Budi", AlamatContoh());
        biasa.Pinjam("X");
        biasa.Pinjam("Y");
        Assert.Throws<InvalidOperationException>(() => biasa.Pinjam("Z"));

        Assert.ThrowsAny<ArgumentException>(() => BuatMahasiswa().Pinjam(""));
    }

    // ---------- Level 6 ----------

    [Fact(DisplayName = "Level 6 - Pewarisan bertingkat: kelas Asisten (sealed)")]
    public void Level6_Asisten()
    {
        var tipe = typeof(Anggota).Assembly.GetType("Pertemuan05.Asisten");
        Assert.True(tipe is not null, "Kelas Asisten belum dibuat (tulis di src/Asisten.cs, namespace Pertemuan05)");

        Assert.True(tipe!.IsSealed, "Asisten harus `sealed`");
        Assert.Equal(typeof(Mahasiswa), tipe.BaseType);

        object? obj;
        try
        {
            obj = Activator.CreateInstance(tipe, "S01", "Rina", AlamatContoh(), "5025201002", "Informatika", "PBO");
        }
        catch (MissingMethodException)
        {
            throw new Xunit.Sdk.XunitException(
                "Konstruktor Asisten(string id, string nama, Alamat alamat, string nrp, string prodi, string mataKuliah) tidak ditemukan");
        }

        var asisten = Assert.IsAssignableFrom<Mahasiswa>(obj);
        Assert.IsAssignableFrom<Anggota>(obj);

        // data warisan dari dua tingkat di atasnya
        Assert.Equal("S01", asisten.Id);
        Assert.Equal("5025201002", asisten.Nrp);
        Assert.Equal(5, asisten.BatasPinjam);

        var mataKuliah = tipe.GetProperty("MataKuliah")?.GetValue(obj);
        Assert.Equal("PBO", mataKuliah);

        var info = tipe.GetMethod("InfoAsisten")?.Invoke(obj, null) as string;
        Assert.NotNull(info);
        Assert.Contains("S01 - Rina", info);
        Assert.Contains("5025201002", info);
        Assert.Contains("PBO", info);

        // aturan pinjam ikut BatasPinjam = 5
        for (int i = 1; i <= 5; i++) asisten.Pinjam($"Buku {i}");
        Assert.Throws<InvalidOperationException>(() => asisten.Pinjam("Buku 6"));
    }

    // ---------- Level 7 ----------

    [Fact(DisplayName = "Level 7 - InfoLengkap() memakai warisan + komposisi")]
    public void Level7_InfoLengkap()
    {
        var m = BuatMahasiswa("M01", "Sari");
        string im = m.InfoLengkap();
        Assert.Equal("M01 - Sari | NRP: 5025201001 | Prodi: Informatika | Alamat: Jl. Mawar 5, Surabaya", im);

        var d = BuatDosen("D01", "Pak Budi");
        string id = d.InfoLengkap();
        Assert.Equal("D01 - Pak Budi | NIP: 198001012005011001 | Alamat: Jl. Mawar 5, Surabaya", id);
    }

    // ---------- Level 8 ----------

    [Fact(DisplayName = "Level 8 - Perpustakaan menampung Anggota (is-a) lewat komposisi")]
    public void Level8_PerpustakaanMenampungAnggota()
    {
        var perpus = new Perpustakaan();
        Assert.Equal(0, perpus.JumlahAnggota);

        perpus.Daftarkan(BuatMahasiswa("M01", "Sari"));
        perpus.Daftarkan(BuatMahasiswa("M02", "Tono"));
        perpus.Daftarkan(BuatDosen("D01", "Pak Budi"));
        perpus.Daftarkan(new Anggota("A01", "Umum", AlamatContoh()));

        Assert.Equal(4, perpus.JumlahAnggota);
        Assert.Equal(2, perpus.JumlahMahasiswa());
        Assert.Equal(1, perpus.JumlahDosen());

        Assert.Throws<InvalidOperationException>(() => perpus.Daftarkan(BuatDosen("M01", "Kembar")));
        Assert.Throws<ArgumentNullException>(() => perpus.Daftarkan(null!));

        var ketemu = perpus.Cari("D01");
        Assert.IsType<Dosen>(ketemu);
        Assert.Null(perpus.Cari("X99"));

        // Asisten adalah Mahasiswa -> ikut terhitung (kalau kelas Asisten sudah dibuat di Level 6)
        var tipeAsisten = typeof(Anggota).Assembly.GetType("Pertemuan05.Asisten");
        if (tipeAsisten is not null)
        {
            var asisten = (Anggota)Activator.CreateInstance(
                tipeAsisten, "S01", "Rina", AlamatContoh(), "5025201002", "Informatika", "PBO")!;
            perpus.Daftarkan(asisten);
            Assert.Equal(3, perpus.JumlahMahasiswa());
        }
    }

    // ---------- Level 9 ----------

    [Fact(DisplayName = "Level 9 - Komposisi: tiap Anggota memiliki LogAktivitas sendiri")]
    public void Level9_LogMilikSendiri()
    {
        var m = BuatMahasiswa("M01", "Sari");
        var d = BuatDosen("D01", "Pak Budi");
        Assert.Empty(m.Riwayat);

        m.Pinjam("Laskar Pelangi");
        m.Pinjam("Bumi Manusia");
        d.Pinjam("Clean Code");

        Assert.Equal(new[] { "Pinjam: Laskar Pelangi", "Pinjam: Bumi Manusia" }, m.Riwayat);
        Assert.Equal(new[] { "Pinjam: Clean Code" }, d.Riwayat);   // log TIDAK dipakai bersama

        // peminjaman yang ditolak tidak tercatat
        var penuh = new Anggota("A01", "Budi", AlamatContoh());
        penuh.Pinjam("A"); penuh.Pinjam("B");
        Assert.Throws<InvalidOperationException>(() => penuh.Pinjam("C"));
        Assert.Equal(2, penuh.Riwayat.Count);

        // log tidak boleh ada sebagai field publik / tidak bisa dimodifikasi lewat Riwayat
        Assert.Empty(typeof(Anggota).GetFields(BindingFlags.Public | BindingFlags.Instance));
        var prop = typeof(Anggota).GetProperty("Riwayat")!;
        Assert.Null(prop.GetSetMethod(nonPublic: false));
    }

    // ---------- Level 10 (bonus) ----------

    private const string PenandaJawaban = "<!-- TULIS JAWABAN KALIAN DI BAWAH BARIS INI -->";

    [Fact(DisplayName = "Level 10 - Bonus: Diagram UML hierarki & komposisi")]
    public void Level10_DiagramHierarki()
    {
        string isi = BacaFileDiFolderPertemuan("NOTASI-HIERARKI.md");
        Assert.False(string.IsNullOrWhiteSpace(isi), "NOTASI-HIERARKI.md tidak ditemukan atau kosong");

        int idx = isi.IndexOf(PenandaJawaban, StringComparison.Ordinal);
        Assert.True(idx >= 0, "Baris penanda tidak ditemukan di NOTASI-HIERARKI.md — jangan hapus baris itu");

        string jawaban = isi[(idx + PenandaJawaban.Length)..].Trim();
        Assert.True(jawaban.Length > 200, "Bagian jawaban di NOTASI-HIERARKI.md masih terlalu pendek/kosong");

        string rendah = jawaban.ToLowerInvariant();
        foreach (var kelas in new[] { "anggota", "mahasiswa", "dosen", "asisten", "alamat", "logaktivitas" })
            Assert.Contains(kelas, rendah.Replace(" ", ""));

        string[] penandaWarisan =
        {
            "<|", "▲", "△", "^", "extends", "inherit", "mewarisi", "turunan", "generalisasi", "is-a", "is a", "adalah",
        };
        Assert.True(penandaWarisan.Any(rendah.Contains),
            "Tunjukkan HUBUNGAN PEWARISAN (mis. panah segitiga kosong <|--, ▲, atau tulis 'extends'/'turunan dari')");

        string[] penandaKomposisi =
        {
            "◆", "◇", "*--", "o--", "<>", "komposisi", "composition", "agregasi", "aggregation", "has-a", "has a", "memiliki",
        };
        Assert.True(penandaKomposisi.Any(rendah.Contains),
            "Tunjukkan HUBUNGAN KOMPOSISI (mis. berlian ◆, *--, atau tulis 'komposisi'/'memiliki')");

        Assert.Contains("protected", rendah);
        Assert.Contains("#", jawaban);   // simbol visibilitas protected
    }
}
