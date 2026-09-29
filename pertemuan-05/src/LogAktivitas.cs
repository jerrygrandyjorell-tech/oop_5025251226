namespace Pertemuan05;

// SUDAH LENGKAP -- jangan diubah. Objek pencatat yang akan DIMILIKI (komposisi)
// oleh setiap Anggota pada Level 9.
public class LogAktivitas
{
    private readonly List<string> _entri = new();

    public IReadOnlyList<string> Semua => _entri.AsReadOnly();

    public void Catat(string pesan) => _entri.Add(pesan);
}
