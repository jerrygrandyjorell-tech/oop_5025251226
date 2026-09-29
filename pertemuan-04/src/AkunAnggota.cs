// File ini berisi TODO -- lihat SOAL.md untuk kontrak lengkap tiap level.
// Bagian bertanda TODO(Level N) adalah tugas kalian; kode lain sudah
// disediakan. Jangan mengubah nama/tipe yang sudah ada kecuali TODO
// memintanya secara eksplisit.

using System;

namespace Pertemuan04;

public class AkunAnggota
{
    public const int MaksPinjaman = 3;

    public string NomorAnggota { get; }

    // Level 9: Nama menggunakan init, Denda dengan setter private
    public string Nama { get; init; } = "";
    public int Denda { get; private set; }

    // Level 10: JumlahPinjamanAktif dengan setter private
    public int JumlahPinjamanAktif { get; private set; }

    public AkunAnggota(string nomorAnggota)
    {
        // Level 9: nomorAnggota null/kosong/spasi -> ArgumentException; selain itu isi NomorAnggota.
        if (string.IsNullOrWhiteSpace(nomorAnggota))
        {
            throw new ArgumentException("Nomor anggota tidak boleh null, kosong, atau hanya berisi spasi.", nameof(nomorAnggota));
        }
        NomorAnggota = nomorAnggota;
    }

    public void TambahDenda(int rupiah)
    {
        // Level 9: rupiah <= 0 -> ArgumentOutOfRangeException; selain itu tambahkan ke Denda.
        if (rupiah <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(rupiah), "Jumlah denda yang ditambah harus lebih dari 0.");
        }
        Denda += rupiah;
    }

    public int BayarDenda(int rupiah)
    {
        // Level 9: rupiah <= 0 -> ArgumentOutOfRangeException
        if (rupiah <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(rupiah), "Jumlah pembayaran denda harus lebih dari 0.");
        }

        // Level 9: rupiah > Denda -> InvalidOperationException (denda tidak berubah)
        if (rupiah > Denda)
        {
            throw new InvalidOperationException("Jumlah pembayaran melebihi total denda saat ini.");
        }

        // Kurangi Denda dan kembalikan sisa denda
        Denda -= rupiah;
        return Denda;
    }

    // Level 10: Ubah modifier menjadi internal agar hanya bisa dipanggil dalam assembly yang sama (Perpustakaan)
    internal void CatatPinjam()
    {
        // Level 10: naikkan JumlahPinjamanAktif satu.
        JumlahPinjamanAktif++;
    }

    internal void CatatKembali()
    {
        // Level 10: turunkan JumlahPinjamanAktif satu (tidak boleh di bawah 0).
        if (JumlahPinjamanAktif > 0)
        {
            JumlahPinjamanAktif--;
        }
    }
}
