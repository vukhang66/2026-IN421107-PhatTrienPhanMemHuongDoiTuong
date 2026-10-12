# User Stories - Hệ thống EduHub

Tài liệu này tổng hợp các User Stories theo chuẩn "As a [role], I want [feature], so that [value]" kèm theo các tiêu chí nghiệm thu (Acceptance Criteria) cho hệ thống quản lý câu lạc bộ & sự kiện EduHub.

---

## 1. Phân loại: Sinh viên

### US01: Đăng ký tham gia câu lạc bộ
* **Story:** As a **Sinh viên**, I want **đăng ký tham gia một câu lạc bộ**, so that **tôi có thể trở thành thành viên chính thức và tham gia các hoạt động nội bộ.**
* **Acceptance Criteria:**
  * **Given** sinh viên đang ở trang thông tin chi tiết của câu lạc bộ, **When** sinh viên nhấn nút "Đăng ký tham gia" và cung cấp mã sinh viên hợp lệ, **Then** hệ thống ghi nhận thông tin và thêm sinh viên vào danh sách thành viên của CLB.
  * **Given** sinh viên đã là thành viên của câu lạc bộ đó, **When** sinh viên thực hiện đăng ký lại, **Then** hệ thống hiển thị thông báo lỗi từ chối thao tác trùng lặp.

### US02: Đăng ký tham gia sự kiện và Check-in
* **Story:** As a **Sinh viên**, I want **đăng ký tham gia sự kiện do CLB tổ chức và thực hiện check-in khi đến nơi**, so that **tôi được ghi nhận tham dự hoạt động và cộng điểm rèn luyện.**
* **Acceptance Criteria:**
  * **Given** sự kiện vẫn còn chỗ trống (`Capacity`), **When** sinh viên gửi yêu cầu đăng ký sự kiện, **Then** hệ thống tạo phiếu đăng ký (`Registration`) với trạng thái `Pending`[cite: 2, 3].
  * **Given** sinh viên đã có phiếu đăng ký, **When** sinh viên thực hiện quét mã hoặc xác nhận check-in tại sự kiện, **Then** hệ thống tạo bản ghi `CheckIn` thành công[cite: 2, 3].

---

## 2. Phân loại: Ban CLB

### US03: Tạo sự kiện mới
* **Story:** As a **Ban CLB**, I want **tạo sự kiện mới cho câu lạc bộ của mình**, so that **các thành viên trong trường có thể nắm bắt thông tin lịch trình và đăng ký tham dự[cite: 2].**
* **Acceptance Criteria:**
  * **Given** ban chủ nhiệm cung cấp đầy đủ thông tin hợp lệ (tiêu đề, thời gian ở tương lai, sức chứa $> 0$), **When** nhấn nút "Tạo sự kiện", **Then** sự kiện được lưu trữ vào hệ thống[cite: 2, 3].
  * **Given** nhập thời gian sự kiện nằm trong quá khứ hoặc sức chứa nhỏ hơn hoặc bằng 0, **When** bấm lưu, **Then** hệ thống báo lỗi không hợp lệ dữ liệu đầu vào[cite: 2, 3].

### US04: Xem danh sách thành viên và xuất báo cáo
* **Story:** As a **Ban CLB**, I want **xem danh sách thành viên và xuất báo cáo thống kê**, so that **tôi có thể quản lý nhân sự và theo dõi tiến độ tham gia của sinh viên[cite: 2].**
* **Acceptance Criteria:**
  * **Given** ban chủ nhiệm truy cập vào trang quản trị CLB, **When** chọn xem mục thành viên, **Then** hệ thống liệt kê danh sách toàn bộ `Member` kèm mã sinh viên và trạng thái hoạt động[cite: 2, 3].
  * **Given** ban chủ nhiệm cần lưu trữ dữ liệu ngoại tuyến, **When** nhấn nút "Xuất báo cáo", **Then** hệ thống tải xuống file dữ liệu chi tiết[cite: 2].

---

## 3. Phân loại: Admin

### US05: Quản lý câu lạc bộ và phê duyệt thành viên
* **Story:** As a **Admin**, I want **quản lý thông tin các câu lạc bộ và phê duyệt thành viên**, so that **đảm bảo hệ thống hoạt động đúng quy chế của trường[cite: 2].**
* **Acceptance Criteria:**
  * **Given** hệ thống có các câu lạc bộ được khởi tạo, **When** admin thực hiện cập nhật thông tin hoặc khóa câu lạc bộ vi phạm, **Then** hệ thống lưu lại trạng thái mới[cite: 2].
  * **Given** có các yêu cầu tham gia CLB đang ở trạng thái chờ duyệt, **When** admin/ban quản lý thực hiện phê duyệt, **Then** trạng thái được chuyển sang chính thức[cite: 2, 3].