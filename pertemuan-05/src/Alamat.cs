// File ini berisi TODO -- lihat SOAL.md untuk kontrak lengkap tiap level.
// Bagian bertanda TODO(Level N) adalah tugas kalian; kode lain sudah
// disediakan. Jangan mengubah nama/tipe yang sudah ada kecuali TODO
// memintanya secara eksplisit.

namespace Pertemuan05;

// Komposisi ("has-a"): sebuah Anggota MEMILIKI sebuah Alamat.
public class Alamat
{
    public string Jalan { get; }
    public string Kota { get; }

    public Alamat(string jalan, string kota)
    {
        // TODO(Level 1): jalan atau kota null/kosong/spasi -> ArgumentException;
        //   selain itu isi Jalan dan Kota.
        throw new NotImplementedException("Level 1 belum diimplementasikan");
    }

    public override string ToString()
    {
        // TODO(Level 1): kembalikan "<Jalan>, <Kota>" (contoh: "Jl. Mawar 5,
        //   Surabaya").
        throw new NotImplementedException("Level 1 belum diimplementasikan");
    }
}
