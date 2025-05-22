// using Mio.ELearning.Service.Models;
// using Mio.ELearning.Service.Models.ViewModels;
// using Mio.ELearning.Service.UnitOfWorks;
//
// namespace Mio.ELearning.Service.Services.Impl;
//
// public class SectionService : ISectionService
// {
//     private readonly IUnitOfWork _unitOfWork;
//     
//     public SectionService(IUnitOfWork unitOfWork)
//     {
//         _unitOfWork = unitOfWork;
//     }
//
//     public async Task<ResultViewModel<List<SectionViewModel>>> GetAllSectionsAsync(int courseId)
//     {
//         var sections = await _unitOfWork.Sections.GetAllWithLessonsAsync(courseId);
//         var result = sections.Select(s => new SectionViewModel
//         {
//             SectionId = s.SectionId,
//             SectionName = s.SectionName,
//             Description = s.Description,
//             Order = s.Order,
//             CourseId = s.CourseId,
//             Lessons = s.Lessons?.Select(l => new LessonViewModel
//             {
//                 LessonId = l.LessonId,
//                 LessonName = l.LessonName,
//                 Description = l.Description,
//                 Content = l.Content,
//                 VideoUrl = l.VideoUrl,
//                 DocumentUrl = l.DocumentUrl,
//                 Order = l.Order,
//                 SectionId = l.SectionId
//             }).ToList() ?? new List<LessonViewModel>()
//         }).ToList();
//
//         return ResultViewModel<List<SectionViewModel>>.Success(result, "Lấy danh sách phần thành công");
//     }
//
//     public async Task<ResultViewModel<SectionViewModel>> GetSectionByIdAsync(int id)
//     {
//         var section = await _unitOfWork.Sections.GetByIdWithLessonsAsync(id);
//         if (section == null)
//             return ResultViewModel<SectionViewModel>.Failure("Không tìm thấy phần");
//
//         var model = new SectionViewModel
//         {
//             SectionId = section.SectionId,
//             SectionName = section.SectionName,
//             Description = section.Description,
//             Order = section.Order,
//             CourseId = section.CourseId,
//             Lessons = section.Lessons?.Select(l => new LessonViewModel
//             {
//                 LessonId = l.LessonId,
//                 LessonName = l.LessonName,
//                 Description = l.Description,
//                 Content = l.Content,
//                 VideoUrl = l.VideoUrl,
//                 DocumentUrl = l.DocumentUrl,
//                 Order = l.Order,
//                 SectionId = l.SectionId
//             }).ToList() ?? new List<LessonViewModel>()
//         };
//
//         return ResultViewModel<SectionViewModel>.Success(model, "Lấy chi tiết phần thành công");
//     }
//
//     public async Task<ResultViewModel<SectionViewModel>> AddSectionAsync(SectionViewModel model)
//     {
//         var course = await _unitOfWork.Courses.GetByIdAsync(model.CourseId);
//         if (course == null)
//             return ResultViewModel<SectionViewModel>.Failure("Khóa học không tồn tại");
//
//         var section = new Section()
//         {
//             SectionName = model.SectionName,
//             Description = model.Description,
//             Order = model.Order,
//             CourseId = model.CourseId,
//             CreatedAt = DateTime.Now,
//             UpdatedAt = DateTime.Now,
//             IsActive = true,
//             IsDeleted = false
//         };
//
//         await _unitOfWork.Sections.AddAsync(section);
//         await _unitOfWork.CompleteAsync();
//
//         model.SectionId = section.SectionId;
//         return ResultViewModel<SectionViewModel>.Success(model, "Tạo phần thành công");
//     }
//
//     public async Task<ResultViewModel<SectionViewModel>> UpdateSectionAsync(SectionViewModel model)
//     {
//         var existing = await _unitOfWork.Sections.GetByIdAsync(model.SectionId);
//         if (existing == null)
//             return ResultViewModel<SectionViewModel>.Failure("Không tìm thấy phần");
//
//         existing.SectionName = model.SectionName;
//         existing.Description = model.Description;
//         existing.Order = model.Order;
//         existing.UpdatedAt = DateTime.Now;
//
//         await _unitOfWork.CompleteAsync();
//         return ResultViewModel<SectionViewModel>.Success(model, "Cập nhật phần thành công");
//     }
//
//     public async Task<ResultViewModel<bool>> DeleteSectionAsync(int id)
//     {
//         var section = await _unitOfWork.Sections.GetByIdAsync(id);
//         if (section == null)
//             return ResultViewModel<bool>.Failure("Không tìm thấy phần");
//
//         section.IsDeleted = true;
//         await _unitOfWork.CompleteAsync();
//         return ResultViewModel<bool>.Success(true, "Xóa phần thành công");
//     }
// }