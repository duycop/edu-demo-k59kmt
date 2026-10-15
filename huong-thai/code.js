// Hàm tải danh sách giáo viên từ API
async function loadTeachers() {
    // Tìm thẻ container chứa danh sách giáo viên trong section #giao-vien
    const teacherContainer = document.querySelector('#giao-vien .grid.md\\:grid-cols-3');
    if (!teacherContainer) return;

    try {
        const response = await fetch('/api.aspx?action=gv');
        if (!response.ok) {
            throw new Error(`HTTP error! status: ${response.status}`);
        }
        const teachers = await response.json();

        // Xóa nội dung mẫu cũ
        teacherContainer.innerHTML = '';

        // Lặp qua danh sách trả về và tạo HTML
        teachers.forEach(teacher => {
            const teacherCard = document.createElement('div');
            teacherCard.className = 'bg-white rounded-2xl overflow-hidden shadow-sm border border-slate-200 text-center p-6';

            teacherCard.innerHTML = `
                <div class="w-24 h-24 rounded-full mx-auto mb-4 overflow-hidden bg-slate-200 border-2 border-brand-500">
                    <img src="${teacher.image}" alt="${teacher.hoten}" class="w-full h-full object-cover" onerror="this.src='https://via.placeholder.com/150'">
                </div>
                <h3 class="font-bold text-slate-900 text-lg">${teacher.hoten}</h3>
                <p class="text-brand-600 text-xs font-semibold mb-3">${teacher.cm}</p>
                <p class="text-slate-500 text-xs leading-relaxed">${teacher.kn}</p>
            `;

            teacherContainer.appendChild(teacherCard);
        });
    } catch (error) {
        console.error('Lỗi khi tải danh sách giáo viên:', error);
    }
}


async function loadSetting() {
    try {
        const response = await fetch('/api.aspx?action=setting');
        if (!response.ok) {
            throw new Error(`HTTP error! status: ${response.status}`);
        }
        const settings = await response.json();
        settings.forEach(st => {
            document.getElementById(st.key).innerHTML = st.html;
        });

    } catch (error) {
        console.error('Lỗi khi tải setting:', error);
    }
}
// Gọi hàm sau khi trang đã tải xong
document.addEventListener('DOMContentLoaded', () => {
    switchTab('trang-chu');
    loadSetting();
    loadTeachers();
});