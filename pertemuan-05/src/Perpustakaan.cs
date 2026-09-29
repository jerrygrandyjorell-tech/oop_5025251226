// File ini berisi TODO -- lihat SOAL.md untuk kontrak lengkap tiap level.
// Bagian bertanda TODO(Level N) adalah tugas kalian; kode lain sudah
// disediakan. Jangan mengubah nama/tipe yang sudah ada kecuali TODO
// memintanya secara eksplisit.

namespace Pertemuan05;

// Komposisi: Perpustakaan MEMILIKI kumpulan Anggota. Karena Mahasiswa dan Dosen
// adalah Anggota, satu daftar bertipe Anggota bisa menampung keduanya.
public class Perpustakaan
{
    private readonly List<Anggota> _anggota = new();

    public int JumlahAnggota => _anggota.Count;

    public void Daftarkan(Anggota anggota)
    {
        // TODO(Level 8): anggota null -> ArgumentNullException; Id yang sudah
        //   terdaftar -> InvalidOperationException; selain itu tambahkan ke
        //   daftar.
        throw new NotImplementedException("Level 8 belum diimplementasikan");
    }

    public Anggota? Cari(string id)
    {
        // TODO(Level 8): kembalikan anggota dengan Id yang sama persis, atau
        //   null.
        throw new NotImplementedException("Level 8 belum diimplementasikan");
    }

    public int JumlahMahasiswa()
    {
        // TODO(Level 8): hitung anggota yang bertipe Mahasiswa (termasuk
        //   turunannya, mis. Asisten -- ingat: turunan "adalah sebuah"
        //   Mahasiswa). Petunjuk: `is` atau OfType<T>().
        throw new NotImplementedException("Level 8 belum diimplementasikan");
    }

    public int JumlahDosen()
    {
        // TODO(Level 8): hitung anggota yang bertipe Dosen.
        throw new NotImplementedException("Level 8 belum diimplementasikan");
    }
}
