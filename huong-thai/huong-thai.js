// FUNCTION CHUYỂN TAB (TAB SEPARATION SCRIPT)
function switchTab(tabId) {
    const allSections = document.querySelectorAll('.page-section');
    const navLinks = document.querySelectorAll('.nav-link');

    // Bỏ active ở tất cả các nút menu
    navLinks.forEach(link => link.classList.remove('active-nav'));

    // Gắn class active cho nút đang chọn
    const activeNav = document.getElementById(`nav-${tabId}`);
    if (activeNav) activeNav.classList.add('active-nav');

    if (tabId === 'trang-chu') {
        // Trang chủ: Hiển thị tất cả các section
        allSections.forEach(section => section.style.display = 'block');
    } else {
        // Các tab khác: Ẩn tất cả và chỉ hiện section tương ứng
        allSections.forEach(section => {
            if (section.id === tabId) {
                section.style.display = 'block';
            } else {
                section.style.display = 'none';
            }
        });
    }

    // Cuộn mượt lên đầu trang
    window.scrollTo({ top: 0, behavior: 'smooth' });
}

// Mặc định hiển thị Trang chủ khi vừa mở trang
document.addEventListener('DOMContentLoaded', () => {
    switchTab('trang-chu');
});

// Dynamic Mapping Rules:
const gradeSubjects = {
    'Lớp 3': ['Môn Toán'],
    'Lớp 4': ['Môn Toán', 'Môn Tiếng Anh'],
    'Lớp 5': ['Môn Toán', 'Môn Tiếng Anh'],
    'Lớp 6': ['Môn Toán', 'Môn Tiếng Anh', 'Môn Ngữ Văn'],
    'Lớp 7': ['Môn Toán', 'Môn Tiếng Anh', 'Môn Ngữ Văn'],
    'Lớp 8': ['Môn Toán', 'Môn Tiếng Anh'],
    'Lớp 9': ['Môn Toán (Luyện thi vào 10)', 'Môn Tiếng Anh (Luyện thi vào 10)', 'Môn Ngữ Văn (Luyện thi vào 10)']
};

// Filter Grade Levels (All, Tiểu Học, THCS) trong Khóa Học
function filterGrade(level) {
    const cards = document.querySelectorAll('.course-card');
    const buttons = document.querySelectorAll('.tab-btn');

    buttons.forEach(btn => btn.classList.remove('active', 'bg-brand-600', 'text-white'));
    buttons.forEach(btn => btn.classList.add('bg-white', 'text-slate-600'));

    const activeBtn = document.getElementById(`tab-${level}`);
    if (activeBtn) {
        activeBtn.classList.add('active');
    }

    cards.forEach(card => {
        if (level === 'all') {
            card.style.display = 'flex';
        } else if (card.classList.contains(level)) {
            card.style.display = 'flex';
        } else {
            card.style.display = 'none';
        }
    });
}

// Dynamic Subjects for Hero Banner Form
function updateHeroSubjects() {
    const gradeSelect = document.getElementById('heroGradeSelect');
    const subjectSelect = document.getElementById('heroSubjectSelect');
    const selectedGrade = gradeSelect.value;

    subjectSelect.innerHTML = '';

    if (selectedGrade && gradeSubjects[selectedGrade]) {
        subjectSelect.disabled = false;
        subjectSelect.classList.remove('bg-slate-100', 'cursor-not-allowed');

        let defaultOption = document.createElement('option');
        defaultOption.value = '';
        defaultOption.textContent = `-- Chọn môn học cho ${selectedGrade} --`;
        subjectSelect.appendChild(defaultOption);

        gradeSubjects[selectedGrade].forEach(subject => {
            let opt = document.createElement('option');
            opt.value = `${selectedGrade} - ${subject}`;
            opt.textContent = subject;
            subjectSelect.appendChild(opt);
        });
    } else {
        subjectSelect.disabled = true;
        subjectSelect.classList.add('bg-slate-100', 'cursor-not-allowed');
        let defaultOption = document.createElement('option');
        defaultOption.value = '';
        defaultOption.textContent = '-- Vui lòng chọn khối lớp trước --';
        subjectSelect.appendChild(defaultOption);
    }
}

// Hàm mở Modal chung "Đăng Ký Tư Vấn"
function openConsultModal() {
    const modal = document.getElementById('registerModal');
    const modalTitle = document.getElementById('modalTitle');
    const gradeSelect = document.getElementById('modalGradeSelect');

    modalTitle.textContent = "Đăng Ký Tư Vấn";
    gradeSelect.value = "";
    updateModalSubjects();

    modal.classList.remove('hidden');
}

// Hàm mở Modal cho từng lớp cụ thể
function openModal(gradeName) {
    const modal = document.getElementById('registerModal');
    const modalTitle = document.getElementById('modalTitle');
    const gradeSelect = document.getElementById('modalGradeSelect');

    modalTitle.textContent = `Đăng Ký Học ${gradeName}`;
    gradeSelect.value = gradeName;
    updateModalSubjects();

    modal.classList.remove('hidden');
}

// Cập nhật môn học theo lớp trong Modal
function updateModalSubjects() {
    const gradeSelect = document.getElementById('modalGradeSelect');
    const subjectSelect = document.getElementById('modalSubjectSelect');
    const hiddenGradeInput = document.getElementById('modalSelectedGrade');

    const selectedGrade = gradeSelect.value;
    hiddenGradeInput.value = selectedGrade;
    subjectSelect.innerHTML = '';

    if (selectedGrade && gradeSubjects[selectedGrade]) {
        subjectSelect.disabled = false;
        subjectSelect.classList.remove('bg-slate-100', 'cursor-not-allowed');

        let defaultOption = document.createElement('option');
        defaultOption.value = '';
        defaultOption.textContent = `-- Chọn môn học cho ${selectedGrade} --`;
        subjectSelect.appendChild(defaultOption);

        gradeSubjects[selectedGrade].forEach(subject => {
            let opt = document.createElement('option');
            opt.value = `${selectedGrade} - ${subject}`;
            opt.textContent = subject;
            subjectSelect.appendChild(opt);
        });
    } else {
        subjectSelect.disabled = true;
        subjectSelect.classList.add('bg-slate-100', 'cursor-not-allowed');
        let defaultOption = document.createElement('option');
        defaultOption.value = '';
        defaultOption.textContent = '-- Vui lòng chọn khối lớp trước --';
        subjectSelect.appendChild(defaultOption);
    }
}

function closeModal() {
    document.getElementById('registerModal').classList.add('hidden');
}

function toggleMobileMenu() {
    const mobileMenu = document.getElementById('mobileMenu');
    mobileMenu.classList.toggle('hidden');
}

async function handleFormSubmit(event) {
    event.preventDefault();
    // event.target chính là thẻ <form> vừa kích hoạt sự kiện submit
    const form = event.target;
    function get_value(selector) {
        var x = form.querySelector(selector);
        if (x) return x.value || '';
        return '';
    }
    // Tìm div.txt-ho-ten nằm BÊN TRONG form này
    const hoten = get_value('.txt-ho-ten-phu-huynh');
    const sdt = get_value('.txt-sdt-phu-huynh');
    const nhucau = get_value('.txt-nhu-cau-tu-van');
    const khoi = get_value('.txt-khoi');
    const mon = get_value('.txt-mon');

    const formData = new URLSearchParams();
    formData.append('hoten', hoten);
    formData.append('sdt', sdt);
    formData.append('nhucau', nhucau);
    formData.append('khoi', khoi);
    formData.append('mon', mon);

    try {
        const response = await fetch('api.aspx?action=bome', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/x-www-form-urlencoded; charset=UTF-8'
            },
            body: formData,
        });

        const result = await response.json();
        if (result.ok) {
            alert('Đăng ký thành công! Trung tâm Thái Hương sẽ liên hệ tư vấn trong thời gian sớm nhất.');
            form.reset();
            if (typeof closeModal === 'function') closeModal();
        } else {
            alert('Có lỗi xảy ra: ' + result.msg);
        }
    } catch (error) {
        console.error('Lỗi khi gửi dữ liệu:', error);
        alert('Không thể kết nối đến máy chủ.');
    }
    event.target.reset();
    closeModal();
}