# Pertemuan 5 — Composition & Inheritance: Anggota Perpustakaan

Tugas ini melatih dua cara menyusun kelas: **komposisi** (*has-a* — sebuah objek **memiliki** objek lain) dan **pewarisan tunggal** (*is-a* — sebuah kelas **adalah jenis khusus** dari kelas lain). Studi kasusnya: anggota perpustakaan — `Anggota`, `Mahasiswa`, `Dosen`, `Asisten`, ditambah `Alamat` dan `LogAktivitas` sebagai objek yang dimiliki.

Semua level dicek otomatis lewat `dotnet test`. Nilai mengikuti level tertinggi yang lolos **berurutan** dari 1 — kerjakan sejauh kemampuan, boleh tidak berurutan (jalankan `dotnet test pertemuan-05/tests -v normal` untuk lihat status tiap level apa adanya).

**Struktur folder yang wajib diikuti** (sudah disiapkan di starter, jangan diubah namanya):
```
pertemuan-05/
  SOAL.md
  NOTASI-HIERARKI.md         <- isi di Level 10
  src/
    Pertemuan05.csproj
    Alamat.cs                <- edit (Level 1)
    LogAktivitas.cs          <- SUDAH LENGKAP, jangan diubah
    Anggota.cs               <- edit (Level 2, 4, 5, 9)
    Mahasiswa.cs             <- edit (Level 3, 4, 7)
    Dosen.cs                 <- edit (Level 3, 4, 7)
    Asisten.cs               <- kosong: TULIS KELAS DARI NOL (Level 6)
    Perpustakaan.cs          <- edit (Level 8)
  tests/
    Pertemuan05.Tests.csproj
    PewarisanTests.cs         <- JANGAN diubah, ini test grading dosen
```

Bagian yang harus kalian kerjakan ditandai komentar `TODO(Level N)`. Kerangka kelas `Mahasiswa` dan `Dosen` (termasuk `: Anggota` dan `: base(...)`) sudah disediakan; **`Asisten` tidak** — kalian menuliskannya sendiri.

---

## Level 1 — Komposisi: Kelas `Alamat`

Lengkapi `Alamat(string jalan, string kota)`: keduanya wajib terisi (`null`/kosong/spasi → `ArgumentException`), lalu simpan di properti get-only `Jalan` dan `Kota`. Override `ToString()` supaya mengembalikan `"<Jalan>, <Kota>"` — mis. `"Jl. Mawar 5, Surabaya"`.

## Level 2 — `Anggota` Memiliki `Alamat` (has-a)

Lengkapi konstruktor `Anggota(string id, string nama, Alamat alamat)`:
- `id` atau `nama` `null`/kosong/spasi → `ArgumentException`.
- `alamat` `null` → `ArgumentNullException`.
- Selain itu isi `Id`, `Nama`, dan `Alamat` (simpan **objek `Alamat` yang sama**, jangan disalin).

Lengkapi juga `Info()` → `"<Id> - <Nama>"`, mis. `"A01 - Budi"`.

## Level 3 — Pewarisan & Rantai Konstruktor

`Mahasiswa` dan `Dosen` sudah dideklarasikan sebagai turunan `Anggota` dan sudah meneruskan tiga parameter pertamanya ke konstruktor induk lewat `: base(id, nama, alamat)`. Tugas kalian mengisi data **khusus** tiap jenis:

- `Mahasiswa`: `Nrp` dan `Prodi` — `null`/kosong/spasi → `ArgumentException`.
- `Dosen`: `Nip` — `null`/kosong/spasi → `ArgumentException`.

Perhatikan bahwa `Id`, `Nama`, dan `Info()` **tidak** ditulis ulang di `Mahasiswa`/`Dosen` — semuanya diwarisi dari `Anggota`. Karena mahasiswa *adalah* anggota, `Anggota a = new Mahasiswa(...)` sah dilakukan.

## Level 4 — Modifier `protected`: `BatasPinjam`

Di `Anggota`, properti `BatasPinjam` (maksimal buku yang boleh dipinjam bersamaan) awalnya punya setter `public`. Ubah menjadi **`protected set`** — hanya `Anggota` dan kelas turunannya yang boleh mengubahnya. Nilai awal `Anggota` biasa tetap **2**. Lalu di konstruktor turunan, atur batasnya:

| Jenis | `BatasPinjam` |
|---|---|
| `Anggota` (umum) | 2 |
| `Mahasiswa` | 3 |
| `Dosen` | 10 |

## Level 5 — `Pinjam()` yang Memakai Batas Tiap Jenis

Lengkapi `Anggota.Pinjam(string judul)`:
- `judul` `null`/kosong/spasi → `ArgumentException`.
- `JumlahPinjam` sudah sama dengan `BatasPinjam` → `InvalidOperationException` (jumlah tidak berubah).
- Selain itu naikkan `JumlahPinjam` satu.

Satu method di kelas induk, tetapi perilakunya menyesuaikan jenis objek lewat nilai `BatasPinjam` masing-masing.

## Level 6 — Pewarisan Bertingkat: Tulis Kelas `Asisten`

File `src/Asisten.cs` sengaja kosong. Tulis di sana kelas **`public sealed class Asisten : Mahasiswa`** (namespace `Pertemuan05`) — seorang mahasiswa yang menjadi asisten mata kuliah:

- Konstruktor `Asisten(string id, string nama, Alamat alamat, string nrp, string prodi, string mataKuliah)`; lima parameter pertama diteruskan ke konstruktor `Mahasiswa` lewat `: base(...)`.
- Properti get-only `string MataKuliah` (tidak boleh kosong → `ArgumentException`).
- `BatasPinjam` = **5**.
- `string InfoAsisten()` → `"<InfoLengkap()> | Asisten: <MataKuliah>"` (`InfoLengkap()` dari Level 7; kalau Level 7 belum dikerjakan, Level 6 ini tetap boleh lolos selama `InfoAsisten()` memuat `Id - Nama`, `Nrp`, dan `MataKuliah`).
- `sealed`: kelas ini tidak boleh diturunkan lagi.

Hierarkinya kini tiga tingkat: `Anggota` ← `Mahasiswa` ← `Asisten`.

## Level 7 — `InfoLengkap()`: Memakai Warisan + Komposisi

Lengkapi `InfoLengkap()` di `Mahasiswa` dan `Dosen`. Format persis (dipisah ` | `):

- `Mahasiswa`: `"<Info()> | NRP: <Nrp> | Prodi: <Prodi> | Alamat: <Alamat>"`
  → `"M01 - Sari | NRP: 5025201001 | Prodi: Informatika | Alamat: Jl. Mawar 5, Surabaya"`
- `Dosen`: `"<Info()> | NIP: <Nip> | Alamat: <Alamat>"`

`Info()` datang dari kelas induk (warisan), sedangkan `Alamat` berasal dari objek yang **dimiliki** (komposisi) — string alamatnya diambil dari `ToString()` milik `Alamat`.

## Level 8 — `Perpustakaan`: Menampung Berbagai Jenis Anggota

Lengkapi `Perpustakaan.cs`. Karena `Mahasiswa` dan `Dosen` adalah `Anggota`, **satu** `List<Anggota>` bisa menampung semuanya:

- `Daftarkan(Anggota anggota)`: `null` → `ArgumentNullException`; `Id` yang sudah terdaftar → `InvalidOperationException`; selain itu tambahkan.
- `Cari(string id)`: anggota dengan `Id` yang sama persis, atau `null`.
- `JumlahMahasiswa()`: banyaknya anggota bertipe `Mahasiswa` — **termasuk** turunannya (`Asisten` juga *adalah* `Mahasiswa`). Petunjuk: operator `is` atau `OfType<T>()`.
- `JumlahDosen()`: banyaknya anggota bertipe `Dosen`.

## Level 9 — Komposisi Lagi: `LogAktivitas` Milik Tiap Anggota

Kelas `LogAktivitas` sudah lengkap. Di `Anggota`:

- Tambahkan field `private readonly LogAktivitas _log = new();` — **setiap** anggota memiliki log-nya sendiri (bukan satu log bersama).
- Lengkapi properti `Riwayat` (`IReadOnlyList<string>`, tanpa setter) agar mengembalikan isi log itu.
- Di `Pinjam(...)`, setiap peminjaman yang **berhasil** dicatat: `_log.Catat($"Pinjam: {judul}")`. Peminjaman yang ditolak tidak dicatat.

Contoh: setelah `Pinjam("Laskar Pelangi")`, `Riwayat` berisi `["Pinjam: Laskar Pelangi"]`.

## Level 10 — Bonus: Diagram UML Hierarki

Buka `pertemuan-05/NOTASI-HIERARKI.md` dan gambar diagram UML-nya di bawah penanda yang sudah disediakan (format bebas — Mermaid, ASCII, atau daftar bertingkat). Wajib memuat: keenam kelas (`Anggota`, `Mahasiswa`, `Dosen`, `Asisten`, `Alamat`, `LogAktivitas`), hubungan **pewarisan** dan hubungan **komposisi** (jelas siapa memiliki siapa), dan simbol visibilitas — `#` untuk anggota `protected`, `+` untuk `public`, `-` untuk `private`. Jangan hapus baris penanda `<!-- TULIS JAWABAN KALIAN DI BAWAH BARIS INI -->`.
