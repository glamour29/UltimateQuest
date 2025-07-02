using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.Rendering;
using static UnityEngine.Analytics.IAnalytic;

public class ChiSo_ThucThe : MonoBehaviour
{
    public CaiDatChiSo caiDatChiSoMacDinh;


    public NhomTaiNguyenChiSo nguonTaiNguyen;
    public LoaiChiSoTanCong congTanCong;
    public LoaiChiSoPhongThu phongThu;
    public NhomChiSoChinh chiSoChinh;


    public float LaySatThuongNguyenTo (out LoaiNguyenTo nguyento, float tiLePhongTo = 1)
    {
        float satThuongLua = congTanCong.satThuongLua.layDuLieu();
        float satThuongBang= congTanCong.satThuongBang.layDuLieu();
        float satThuongSet = congTanCong.satThuongSet.layDuLieu();
        float bonusSatThuongNguyenTo = chiSoChinh.triTue.layDuLieu(); //bonus elemental damage from intelligence +1 per INIT

        float satThuongCaoNhat = satThuongLua;
        nguyento = LoaiNguyenTo.Lua;

        if (satThuongBang > satThuongCaoNhat)
        {
            satThuongCaoNhat = satThuongBang;
            nguyento = LoaiNguyenTo.Bang;
        }

        if (satThuongSet > satThuongCaoNhat)
        {
            satThuongCaoNhat = satThuongSet;
            nguyento = LoaiNguyenTo.Set;
        }

        if(satThuongCaoNhat <= 0)
        {
            nguyento = LoaiNguyenTo.KhongCo;
            return 0;
        }

        float bonusLua = (satThuongLua == satThuongCaoNhat) ? 0 : satThuongLua * .5f; 
        float bonusBang = (satThuongBang == satThuongCaoNhat) ? 0 : satThuongBang * .5f; 
        float bonusSet = (satThuongSet == satThuongCaoNhat) ? 0 : satThuongSet * .5f;

        float satThuongLenNguyenToYeu = bonusLua + bonusBang + bonusSet;
        float satThuongCuoiCung = satThuongCaoNhat + satThuongLenNguyenToYeu + bonusSatThuongNguyenTo;

        return satThuongCuoiCung * tiLePhongTo;
    }

    public float TinhSatThuongVatLy(out bool BaoKich, float tiLePhongTo = 1 ) 
    {
        // Lấy sát thương cơ bản và cộng thêm từ sức mạnh
        float satThuongCoBan = congTanCong.satThuong.layDuLieu();
        float satThuongCongThem = chiSoChinh.sucManh.layDuLieu();
        float tongSatThuongCoBan = satThuongCoBan + satThuongCongThem;

        // Lấy tỉ lệ bạo kích cơ bản và cộng thêm từ nhanh nhẹn (+0.3% mỗi điểm AGI)
        float tiLeBaoKichCoBan = congTanCong.tiLeChiMang.layDuLieu();
        float tiLeBaoKichCongThem = chiSoChinh.nhanhNhen.layDuLieu() * 0.3f;
        float tongTiLeBaoKich = tiLeBaoKichCoBan + tiLeBaoKichCongThem;

        // Lấy sức mạnh bạo kích cơ bản và cộng thêm từ sức mạnh (+0.5% mỗi điểm STR)
        float sucManhBaoKichCoBan = congTanCong.sucManhChiMang.layDuLieu();
        float sucManhBaoKichCongThem = chiSoChinh.sucManh.layDuLieu() * 0.5f;
        float tongSucManhBaoKich = (sucManhBaoKichCoBan + sucManhBaoKichCongThem) / 100;
        // e.g. 150 / 100 = 1.5 (hệ số nhân)

        // Xác định xem có phải bạo kích không
        BaoKich = Random.Range(0, 100) < tongTiLeBaoKich;

        // Tính sát thương cuối cùng
        float satThuongCuoiCung = BaoKich ? tongSatThuongCoBan * tongSucManhBaoKich : tongSatThuongCoBan;

        return satThuongCuoiCung * tiLePhongTo;
    }

    public float LayKhangNguyenTo(LoaiNguyenTo nguyento)
    {
        float khangCoBan = 0;
        float bonusKhang = chiSoChinh.triTue.layDuLieu() * .5f; //bonus khang tu tri tue 0.5% cho moi VIT

            switch (nguyento)
        {
            case LoaiNguyenTo.Lua:
                khangCoBan = phongThu.khangLua.layDuLieu();
            break;
            case LoaiNguyenTo.Bang:
                khangCoBan = phongThu.khangBang.layDuLieu();
                break;
            case LoaiNguyenTo.Set:
                khangCoBan = phongThu.khangSet.layDuLieu();
                break;
        }
        float khang = khangCoBan + bonusKhang;
        float gioiHanKhang = 75f; // resistance will be capped at 75%
        float khangCuoiCung = Mathf.Clamp (khang,0,gioiHanKhang)/100 ;//convert value into 0 to 1 multiplier
        return khangCuoiCung;
    }

    public float layTiLeGiamSatThuongTuGiap(float GiaTriGiamSatThuong)
    {
        float giapGoc = phongThu.Giap.layDuLieu();
        float giapCongThem = chiSoChinh.theLuc.layDuLieu();// Giáp cộng thêm từ thể lực: +1 mỗi điểm Thể Lực (VIT)
        float tongGiap = giapGoc + giapCongThem;

        float heSoGiamSatThuong =Mathf.Clamp (1 - GiaTriGiamSatThuong,0,1);
        float giapHieuQua = tongGiap * heSoGiamSatThuong;


        float tiLeGiamSatThuong = giapHieuQua / (giapHieuQua + 100);
        float gioiHanTiLeGiamSatThuong = .85f;


        //Mathf.Clamp(giá_trị, giá_trị_thấp_nhất, giá_trị_cao_nhất); // Giới hạn giá trị trong khoảng từ min đến max
        float tiLeGiamSatThuongCuoi = Mathf.Clamp(tiLeGiamSatThuong, 0, gioiHanTiLeGiamSatThuong);

        return tiLeGiamSatThuongCuoi;
    }

    public float layGiaTriGiamSatThuongTuGiap()
    {
        float GiaTriGiamSatThuongCuoiCung = congTanCong.giamGiap.layDuLieu() / 100;
        return GiaTriGiamSatThuongCuoiCung;
    }

    // Tính máu tối đa cuối cùng (gốc + cộng thêm từ thể lực)
    public float layHpToiDa()
    {
        float HpToiDaGoc = nguonTaiNguyen.mauToiDa.layDuLieu();
        float CongThemHpToiDa = chiSoChinh.theLuc.layDuLieu() * 5;
        float HpToiDaCuoiCung = HpToiDaGoc + CongThemHpToiDa;

        return HpToiDaCuoiCung;
    }

    // Tính tỉ lệ né đòn cuối cùng (gốc + cộng thêm từ nhanh nhẹn, giới hạn tối đa)
    public float LayGiaTriNeDon()
    {
        float ChiSoNeDonGoc = phongThu.neTranh.layDuLieu();
        float bonusChiSoNeDon = chiSoChinh.nhanhNhen.layDuLieu() * .5f; //mỗi điểm nhanh nhẹn sẽ cho bạn 0.5% né đòn

        float TongChiSoNeDon = ChiSoNeDonGoc + bonusChiSoNeDon;
        float GioiHanNeDon = 80f;

        float NeDonCuoiCung = Mathf.Clamp(TongChiSoNeDon, 0, GioiHanNeDon);

        return NeDonCuoiCung;

    }
    public ChiSo layChiSoTheoLoai(LoaiChiSo loai)
    {
        switch (loai)
        {
            case LoaiChiSo.MauToiDa: return nguonTaiNguyen.mauToiDa;
            case LoaiChiSo.HoiMau: return nguonTaiNguyen.hoiMau;

            case LoaiChiSo.SucManh: return chiSoChinh.sucManh;
            case LoaiChiSo.NhanhNhen: return chiSoChinh.nhanhNhen;
            case LoaiChiSo.TriTue: return chiSoChinh.triTue;
            case LoaiChiSo.TheLuc: return chiSoChinh.theLuc;

            case LoaiChiSo.TocDoDanh: return congTanCong.tocDoDanh;
            case LoaiChiSo.SatThuong: return congTanCong.satThuong;
            case LoaiChiSo.TiLeChiMang: return congTanCong.tiLeChiMang;
            case LoaiChiSo.SucManhChiMang: return congTanCong.sucManhChiMang;
            case LoaiChiSo.GiamGiap: return congTanCong.giamGiap;

            case LoaiChiSo.SatThuongLua: return congTanCong.satThuongLua;
            case LoaiChiSo.SatThuongBang: return congTanCong.satThuongBang;
            case LoaiChiSo.SatThuongSet: return congTanCong.satThuongSet;

            case LoaiChiSo.Giap: return phongThu.Giap;
            case LoaiChiSo.NeTranh: return phongThu.neTranh;

            case LoaiChiSo.KhangBang: return phongThu.khangBang;
            case LoaiChiSo.KhangLua: return phongThu.khangLua;
            case LoaiChiSo.KhangSet: return phongThu.khangSet;

            default:
                Debug.LogWarning($"LoaiChiSo {loai} chưa được cài đặt.");
                return null;
        }
    }

    [ContextMenu ("Cap Nhat Chi So Mac Dinh")]
    public void ApDungCaiDatChiSoMacDinh()
    {
        if (caiDatChiSoMacDinh == null)
        {
            Debug.Log("no default stat setup assigned");
            return;
        }

        nguonTaiNguyen.mauToiDa.GanGiaTriNen(caiDatChiSoMacDinh.mauToiDa);
        nguonTaiNguyen.hoiMau.GanGiaTriNen(caiDatChiSoMacDinh.hoiMau);
        // Chỉ số chính
        chiSoChinh.sucManh.GanGiaTriNen(caiDatChiSoMacDinh.sucManh);
        chiSoChinh.nhanhNhen.GanGiaTriNen(caiDatChiSoMacDinh.nhanhNhen);
        chiSoChinh.triTue.GanGiaTriNen(caiDatChiSoMacDinh.triTue);
        chiSoChinh.theLuc.GanGiaTriNen(caiDatChiSoMacDinh.theLuc);

        // Tấn công vật lý
        congTanCong.tocDoDanh.GanGiaTriNen(caiDatChiSoMacDinh.tocDoDanh);
        congTanCong.satThuong.GanGiaTriNen(caiDatChiSoMacDinh.satThuong);
        congTanCong.tiLeChiMang.GanGiaTriNen(caiDatChiSoMacDinh.tiLeChiMang);
        congTanCong.sucManhChiMang.GanGiaTriNen(caiDatChiSoMacDinh.sucManhChiMang);
        congTanCong.giamGiap.GanGiaTriNen(caiDatChiSoMacDinh.giamGiap);

        // Tấn công nguyên tố
        congTanCong.satThuongBang.GanGiaTriNen(caiDatChiSoMacDinh.satThuongBang);
        congTanCong.satThuongLua.GanGiaTriNen(caiDatChiSoMacDinh.satThuongLua);
        congTanCong.satThuongSet.GanGiaTriNen(caiDatChiSoMacDinh.satThuongSet);

        // Phòng thủ vật lý
        phongThu.Giap.GanGiaTriNen(caiDatChiSoMacDinh.giap);
        phongThu.neTranh.GanGiaTriNen(caiDatChiSoMacDinh.neTranh);

        // Phòng thủ nguyên tố
        phongThu.khangBang.GanGiaTriNen(caiDatChiSoMacDinh.khangBang);
        phongThu.khangLua.GanGiaTriNen(caiDatChiSoMacDinh.khangLua);
        phongThu.khangSet.GanGiaTriNen(caiDatChiSoMacDinh.khangSet);
    }

}
