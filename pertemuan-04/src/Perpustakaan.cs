// File ini berisi TODO -- lihat SOAL.md untuk kontrak lengkap tiap level.
// Bagian bertanda TODO(Level N) adalah tugas kalian; kode lain sudah
// disediakan. Jangan mengubah nama/tipe yang sudah ada kecuali TODO
// memintanya secara eksplisit.

using System;
using System.Collections.Generic;
using System.Linq;

namespace Pertemuan04;

public class Perpustakaan
{
    // Level 7: Simpan daftar di field private dan ekspos sebagai IReadOnlyList<Buku>
    private readonly List<Buku> _daftarBuku = new();
    
    public IReadOnlyList<Buku> DaftarBuku => _daftarBuku.AsReadOnly();

    public int JumlahJudul => _daftarBuku.Count;

    public void Tambah(Buku buku)
    {
        // Level 7: buku null -> ArgumentNullException
        if (buku == null)
        {
            throw new ArgumentNullException(nameof(buku), "Buku tidak boleh null.");
        }

        // Level 7: ISBN yang sudah ada di koleksi -> InvalidOperationException
        if (_daftarBuku.Any(b => b.Isbn == buku.Isbn))
        {
            throw new InvalidOperationException("Buku dengan ISBN tersebut sudah terdaftar di perpustakaan.");
        }

        _daftarBuku.Add(buku);
    }

    public Buku? Cari(string isbn)
    {
        // Level 7: kembalikan buku dengan Isbn yang sama persis, atau null kalau tidak ada.
        return _daftarBuku.FirstOrDefault(b => b.Isbn == isbn);
    }

    public void PinjamBuku(string isbn, AkunAnggota akun)
    {
        // Level 10: akun null -> ArgumentNullException
        if (akun == null)
        {
            throw new ArgumentNullException(nameof(akun), "Akun anggota tidak boleh null.");
        }

        // Level 10: ISBN tidak ada -> ArgumentException
        var buku = Cari(isbn);
        if (buku == null)
        {
            throw new ArgumentException("Buku dengan ISBN tersebut tidak ditemukan di perpustakaan.", nameof(isbn));
        }

        // Level 10: akun.Denda > 0 -> InvalidOperationException
        if (akun.Denda > 0)
        {
            throw new InvalidOperationException("Anggota memiliki denda aktif yang belum dibayar.");
        }

        // Level 10: akun.JumlahPinjamanAktif sudah sama dengan AkunAnggota.MaksPinjaman (3) -> InvalidOperationException
        if (akun.JumlahPinjamanAktif >= AkunAnggota.MaksPinjaman)
        {
            throw new InvalidOperationException("Jumlah pinjaman aktif anggota telah mencapai batas maksimum.");
        }

        // Level 10: panggil buku.Pinjam() lalu akun.CatatPinjam()
        buku.Pinjam();
        akun.CatatPinjam();
    }

    public void KembalikanBuku(string isbn, AkunAnggota akun)
    {
        // Level 10: akun null -> ArgumentNullException
        if (akun == null)
        {
            throw new ArgumentNullException(nameof(akun), "Akun anggota tidak boleh null.");
        }

        // Level 10: ISBN tidak ada -> ArgumentException
        var buku = Cari(isbn);
        if (buku == null)
        {
            throw new ArgumentException("Buku dengan ISBN tersebut tidak ditemukan di perpustakaan.", nameof(isbn));
        }

        // Level 10: akun.JumlahPinjamanAktif = 0 -> InvalidOperationException
        if (akun.JumlahPinjamanAktif == 0)
        {
            throw new InvalidOperationException("Anggota tidak memiliki pinjaman aktif buku ini.");
        }

        // Level 10: panggil buku.Kembalikan() lalu akun.CatatKembali()
        buku.Kembalikan();
        akun.CatatKembali();
    }
}
