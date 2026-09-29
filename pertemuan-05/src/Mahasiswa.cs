// File ini berisi TODO -- lihat SOAL.md untuk kontrak lengkap tiap level.
// Bagian bertanda TODO(Level N) adalah tugas kalian; kode lain sudah
// disediakan. Jangan mengubah nama/tipe yang sudah ada kecuali TODO
// memintanya secara eksplisit.

namespace Pertemuan05;

// Pewarisan tunggal: Mahasiswa "adalah sebuah" Anggota.
public class Mahasiswa : Anggota
{
    public string Nrp { get; }
    public string Prodi { get; }

    // TODO(Level 4): Mahasiswa boleh meminjam maksimal 3 buku -- atur
    //   BatasPinjam = 3 di konstruktor ini (setter-nya protected, jadi bisa
    //   diakses dari kelas turunan).
    public Mahasiswa(string id, string nama, Alamat alamat, string nrp, string prodi)
        : base(id, nama, alamat)
    {
        // TODO(Level 3): nrp/prodi null/kosong/spasi -> ArgumentException;
        //   selain itu isi Nrp dan Prodi. (Konstruktor kelas induk sudah
        //   dipanggil lewat `: base(...)` di atas.)
        throw new NotImplementedException("Level 3 belum diimplementasikan");
    }

    public string InfoLengkap()
    {
        // TODO(Level 7): gabungkan Info() milik kelas induk dengan data khusus
        //   mahasiswa, dipisah " | ": "<Info()> | NRP: <Nrp> | Prodi: <Prodi> |
        //   Alamat: <Alamat>" (Alamat memakai ToString() milik objek Alamat).
        throw new NotImplementedException("Level 7 belum diimplementasikan");
    }
}
