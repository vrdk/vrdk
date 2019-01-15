$(document).ready(function () { $(function () { $("#vacrequestmodal__from_datepicker").datepicker({ dayNamesMin: ["Вс", "Пн", "Вт", "Ср", "Чт", "Пт", "Сб"], dateFormat: 'dd.mm.yy', monthNames: ["Январь", "Февраль", "Март", "Апрель", "Май", "Июнь", "Июль", "Август", "Сентябрь", "Октябрь", "Ноябрь", "Декабрь"], firstDay: 1 }), $("#vacrequestmodal__to_datepicker").datepicker({ dayNamesMin: ["Вс", "Пн", "Вт", "Ср", "Чт", "Пт", "Сб"], dateFormat: 'dd.mm.yy', monthNames: ["Январь", "Февраль", "Март", "Апрель", "Май", "Июнь", "Июль", "Август", "Сентябрь", "Октябрь", "Ноябрь", "Декабрь"], firstDay: 1 }), $("#tripaddmodal__from_datepicker").datepicker({ dayNamesMin: ["Вс", "Пн", "Вт", "Ср", "Чт", "Пт", "Сб"], dateFormat: 'dd.mm.yy', monthNames: ["Январь", "Февраль", "Март", "Апрель", "Май", "Июнь", "Июль", "Август", "Сентябрь", "Октябрь", "Ноябрь", "Декабрь"], firstDay: 1 }), $("#tripaddmodal__to_datepicker").datepicker({ dayNamesMin: ["Вс", "Пн", "Вт", "Ср", "Чт", "Пт", "Сб"], dateFormat: 'dd.mm.yy', monthNames: ["Январь", "Февраль", "Март", "Апрель", "Май", "Июнь", "Июль", "Август", "Сентябрь", "Октябрь", "Ноябрь", "Декабрь"], firstDay: 1 }), $("#tripeditmodal__from_datepicker").datepicker({ dayNamesMin: ["Вс", "Пн", "Вт", "Ср", "Чт", "Пт", "Сб"], dateFormat: 'dd.mm.yy', monthNames: ["Январь", "Февраль", "Март", "Апрель", "Май", "Июнь", "Июль", "Август", "Сентябрь", "Октябрь", "Ноябрь", "Декабрь"], firstDay: 1 }), $("#tripeditmodal__to_datepicker").datepicker({ dayNamesMin: ["Вс", "Пн", "Вт", "Ср", "Чт", "Пт", "Сб"], dateFormat: 'dd.mm.yy', monthNames: ["Январь", "Февраль", "Март", "Апрель", "Май", "Июнь", "Июль", "Август", "Сентябрь", "Октябрь", "Ноябрь", "Декабрь"], firstDay: 1 }), $("#empprofileedit__datepicker__birthDate").datepicker({ dayNamesMin: ["Вс", "Пн", "Вт", "Ср", "Чт", "Пт", "Сб"], dateFormat: 'dd.mm.yy', monthNames: ["Январь", "Февраль", "Март", "Апрель", "Май", "Июнь", "Июль", "Август", "Сентябрь", "Октябрь", "Ноябрь", "Декабрь"], firstDay: 1 }), $("#vacrequest__from_datepicker").datepicker({ dayNamesMin: ["Вс", "Пн", "Вт", "Ср", "Чт", "Пт", "Сб"], dateFormat: 'dd.mm.yy', monthNames: ["Январь", "Февраль", "Март", "Апрель", "Май", "Июнь", "Июль", "Август", "Сентябрь", "Октябрь", "Ноябрь", "Декабрь"], firstDay: 1 }), $("#vacrequest__to_datepicker").datepicker({ dayNamesMin: ["Вс", "Пн", "Вт", "Ср", "Чт", "Пт", "Сб"], dateFormat: 'dd.mm.yy', monthNames: ["Январь", "Февраль", "Март", "Апрель", "Май", "Июнь", "Июль", "Август", "Сентябрь", "Октябрь", "Ноябрь", "Декабрь"], firstDay: 1 }), $("#empprofileedit__datepicker__hireDate").datepicker({ dayNamesMin: ["Вс", "Пн", "Вт", "Ср", "Чт", "Пт", "Сб"], dateFormat: 'dd.mm.yy', monthNames: ["Январь", "Февраль", "Март", "Апрель", "Май", "Июнь", "Июль", "Август", "Сентябрь", "Октябрь", "Ноябрь", "Декабрь"], firstDay: 1 }), $("#empprofileedit__datepicker__dismissalDate").datepicker({ dayNamesMin: ["Вс", "Пн", "Вт", "Ср", "Чт", "Пт", "Сб"], dateFormat: 'dd.mm.yy', monthNames: ["Январь", "Февраль", "Март", "Апрель", "Май", "Июнь", "Июль", "Август", "Сентябрь", "Октябрь", "Ноябрь", "Декабрь"], firstDay: 1 }); }), window.onload = function () { !function () { let e = document.getElementsByClassName("profile__tab"), a = document.getElementsByClassName("profile__tab_post"); for (let t = 0; t < e.length; t++)e[t].addEventListener("click", function (i) { i.preventDefault(); for (let t = 0; t < e.length; t++)e[t].classList.remove("profile__tab_active"), a[t].classList.remove("profile__tab_postactive"); e[t].classList.add("profile__tab_active"), a[t].classList.add("profile__tab_postactive"); }); }(); }; });
//# sourceMappingURL=data:application/json;charset=utf8;base64,eyJ2ZXJzaW9uIjozLCJzb3VyY2VzIjpbIm1haW4uanMiXSwibmFtZXMiOlsiJCIsImRhdGVwaWNrZXIiLCJkYXlOYW1lc01pbiIsIm1vbnRoTmFtZXMiLCJmaXJzdERheSIsIndpbmRvdyIsIm9ubG9hZCIsInRhYkxpbmtzIiwiZG9jdW1lbnQiLCJnZXRFbGVtZW50c0J5Q2xhc3NOYW1lIiwidGFiUG9zdHMiLCJjbGFzc0xpc3QiLCJhZGQiLCJpIiwibGVuZ3RoIiwiYWRkRXZlbnRMaXN0ZW5lciIsImUiLCJwcmV2ZW50RGVmYXVsdCIsImoiLCJyZW1vdmUiLCJ0YWJzIl0sIm1hcHBpbmdzIjoiQUFBQ0EsRUFBRyxXQUNBQSxFQUFHLHFDQUFzQ0MsV0FBVyxDQUNuREMsWUFBYSxDQUFFLEtBQU0sS0FBTSxLQUFNLEtBQU0sS0FBTSxLQUFNLE1BQ25EQyxXQUFZLENBQUUsU0FBVSxVQUFXLE9BQVEsU0FBVSxNQUFPLE9BQVEsT0FBUSxTQUFVLFdBQVksVUFBVyxTQUFVLFdBQ3ZIQyxTQUFVLElBRVhKLEVBQUcsbUNBQW9DQyxXQUFXLENBQ2pEQyxZQUFhLENBQUUsS0FBTSxLQUFNLEtBQU0sS0FBTSxLQUFNLEtBQU0sTUFDbkRDLFdBQVksQ0FBRSxTQUFVLFVBQVcsT0FBUSxTQUFVLE1BQU8sT0FBUSxPQUFRLFNBQVUsV0FBWSxVQUFXLFNBQVUsV0FDdkhDLFNBQVUsSUFFWEosRUFBRyxrQ0FBbUNDLFdBQVcsQ0FDaERDLFlBQWEsQ0FBRSxLQUFNLEtBQU0sS0FBTSxLQUFNLEtBQU0sS0FBTSxNQUNuREMsV0FBWSxDQUFFLFNBQVUsVUFBVyxPQUFRLFNBQVUsTUFBTyxPQUFRLE9BQVEsU0FBVSxXQUFZLFVBQVcsU0FBVSxXQUN2SEMsU0FBVSxJQUVYSixFQUFHLGdDQUFpQ0MsV0FBVyxDQUM5Q0MsWUFBYSxDQUFFLEtBQU0sS0FBTSxLQUFNLEtBQU0sS0FBTSxLQUFNLE1BQ25EQyxXQUFZLENBQUUsU0FBVSxVQUFXLE9BQVEsU0FBVSxNQUFPLE9BQVEsT0FBUSxTQUFVLFdBQVksVUFBVyxTQUFVLFdBQ3ZIQyxTQUFVLElBRVhKLEVBQUcsbUNBQW9DQyxXQUFXLENBQ2pEQyxZQUFhLENBQUUsS0FBTSxLQUFNLEtBQU0sS0FBTSxLQUFNLEtBQU0sTUFDbkRDLFdBQVksQ0FBRSxTQUFVLFVBQVcsT0FBUSxTQUFVLE1BQU8sT0FBUSxPQUFRLFNBQVUsV0FBWSxVQUFXLFNBQVUsV0FDdkhDLFNBQVUsSUFFWEosRUFBRyxpQ0FBa0NDLFdBQVcsQ0FDL0NDLFlBQWEsQ0FBRSxLQUFNLEtBQU0sS0FBTSxLQUFNLEtBQU0sS0FBTSxNQUNuREMsV0FBWSxDQUFFLFNBQVUsVUFBVyxPQUFRLFNBQVUsTUFBTyxPQUFRLE9BQVEsU0FBVSxXQUFZLFVBQVcsU0FBVSxXQUN2SEMsU0FBVSxJQUVYSixFQUFHLCtCQUFnQ0MsV0FBVyxDQUM3Q0MsWUFBYSxDQUFFLEtBQU0sS0FBTSxLQUFNLEtBQU0sS0FBTSxLQUFNLE1BQ25EQyxXQUFZLENBQUUsU0FBVSxVQUFXLE9BQVEsU0FBVSxNQUFPLE9BQVEsT0FBUSxTQUFVLFdBQVksVUFBVyxTQUFVLFdBQ3ZIQyxTQUFVLElBRVhKLEVBQUcsZ0NBQWlDQyxXQUFXLENBQzlDQyxZQUFhLENBQUUsS0FBTSxLQUFNLEtBQU0sS0FBTSxLQUFNLEtBQU0sTUFDbkRDLFdBQVksQ0FBRSxTQUFVLFVBQVcsT0FBUSxTQUFVLE1BQU8sT0FBUSxPQUFRLFNBQVUsV0FBWSxVQUFXLFNBQVUsV0FDdkhDLFNBQVUsSUFFWEosRUFBRyw4QkFBK0JDLFdBQVcsQ0FDNUNDLFlBQWEsQ0FBRSxLQUFNLEtBQU0sS0FBTSxLQUFNLEtBQU0sS0FBTSxNQUNuREMsV0FBWSxDQUFFLFNBQVUsVUFBVyxPQUFRLFNBQVUsTUFBTyxPQUFRLE9BQVEsU0FBVSxXQUFZLFVBQVcsU0FBVSxXQUN2SEMsU0FBVSxNQUdmQyxPQUFPQyxPQUFTLFlBQ2YsV0FDSSxJQUFJQyxFQUFXQyxTQUFTQyx1QkFBdUIsZ0JBQzNDQyxFQUFXRixTQUFTQyx1QkFBdUIscUJBQzlCRCxTQUFTQyx1QkFBdUIsNkJBQTZCLEdBRTlFQyxFQUFTLEdBQUdDLFVBQVVDLElBQUksdUJBQzFCLElBQUssSUFBSUMsRUFBSSxFQUFHQSxFQUFJTixFQUFTTyxPQUFRRCxJQUNuQ04sRUFBU00sR0FBR0UsaUJBQWlCLFFBQVMsU0FBU0MsR0FDN0NBLEVBQUVDLGlCQUNGLElBQUssSUFBSUMsRUFBSSxFQUFHQSxFQUFJWCxFQUFTTyxPQUFRSSxJQUNuQ1gsRUFBU1csR0FBR1AsVUFBVVEsT0FBTyx1QkFDN0JULEVBQVNRLEdBQUdQLFVBQVVRLE9BQU8sMkJBRS9CWixFQUFTTSxHQUFHRixVQUFVQyxJQUFJLHVCQUMxQkYsRUFBU0csR0FBR0YsVUFBVUMsSUFBSSw2QkFJbENRIiwiZmlsZSI6Im1haW4uanMiLCJzb3VyY2VzQ29udGVudCI6WyIgJCggZnVuY3Rpb24oKSB7XHJcbiAgICAkKCBcIiN2YWNyZXF1ZXN0bW9kYWxfX2Zyb21fZGF0ZXBpY2tlclwiICkuZGF0ZXBpY2tlcih7XHJcbiAgICBcdGRheU5hbWVzTWluOiBbIFwi0JLRgVwiLCBcItCf0L1cIiwgXCLQktGCXCIsIFwi0KHRgFwiLCBcItCn0YJcIiwgXCLQn9GCXCIsIFwi0KHQsVwiIF0sXHJcbiAgICBcdG1vbnRoTmFtZXM6IFsgXCLQr9C90LLQsNGA0YxcIiwgXCLQpNC10LLRgNCw0LvRjFwiLCBcItCc0LDRgNGCXCIsIFwi0JDQv9GA0LXQu9GMXCIsIFwi0JzQsNC5XCIsIFwi0JjRjtC90YxcIiwgXCLQmNGO0LvRjFwiLCBcItCQ0LLQs9GD0YHRglwiLCBcItCh0LXQvdGC0Y/QsdGA0YxcIiwgXCLQntC60YLRj9Cx0YDRjFwiLCBcItCd0L7Rj9Cx0YDRjFwiLCBcItCU0LXQutCw0LHRgNGMXCIgXSxcclxuICAgIFx0Zmlyc3REYXk6IDFcclxuICAgIH0pO1xyXG4gICAgJCggXCIjdmFjcmVxdWVzdG1vZGFsX190b19kYXRlcGlja2VyXCIgKS5kYXRlcGlja2VyKHtcclxuICAgIFx0ZGF5TmFtZXNNaW46IFsgXCLQktGBXCIsIFwi0J/QvVwiLCBcItCS0YJcIiwgXCLQodGAXCIsIFwi0KfRglwiLCBcItCf0YJcIiwgXCLQodCxXCIgXSxcclxuICAgIFx0bW9udGhOYW1lczogWyBcItCv0L3QstCw0YDRjFwiLCBcItCk0LXQstGA0LDQu9GMXCIsIFwi0JzQsNGA0YJcIiwgXCLQkNC/0YDQtdC70YxcIiwgXCLQnNCw0LlcIiwgXCLQmNGO0L3RjFwiLCBcItCY0Y7Qu9GMXCIsIFwi0JDQstCz0YPRgdGCXCIsIFwi0KHQtdC90YLRj9Cx0YDRjFwiLCBcItCe0LrRgtGP0LHRgNGMXCIsIFwi0J3QvtGP0LHRgNGMXCIsIFwi0JTQtdC60LDQsdGA0YxcIiBdLFxyXG4gICAgXHRmaXJzdERheTogMVxyXG4gICAgfSk7XHJcbiAgICAkKCBcIiN0cmlwYWRkbW9kYWxfX2Zyb21fZGF0ZXBpY2tlclwiICkuZGF0ZXBpY2tlcih7XHJcbiAgICBcdGRheU5hbWVzTWluOiBbIFwi0JLRgVwiLCBcItCf0L1cIiwgXCLQktGCXCIsIFwi0KHRgFwiLCBcItCn0YJcIiwgXCLQn9GCXCIsIFwi0KHQsVwiIF0sXHJcbiAgICBcdG1vbnRoTmFtZXM6IFsgXCLQr9C90LLQsNGA0YxcIiwgXCLQpNC10LLRgNCw0LvRjFwiLCBcItCc0LDRgNGCXCIsIFwi0JDQv9GA0LXQu9GMXCIsIFwi0JzQsNC5XCIsIFwi0JjRjtC90YxcIiwgXCLQmNGO0LvRjFwiLCBcItCQ0LLQs9GD0YHRglwiLCBcItCh0LXQvdGC0Y/QsdGA0YxcIiwgXCLQntC60YLRj9Cx0YDRjFwiLCBcItCd0L7Rj9Cx0YDRjFwiLCBcItCU0LXQutCw0LHRgNGMXCIgXSxcclxuICAgIFx0Zmlyc3REYXk6IDFcclxuICAgIH0pO1xyXG4gICAgJCggXCIjdHJpcGFkZG1vZGFsX190b19kYXRlcGlja2VyXCIgKS5kYXRlcGlja2VyKHtcclxuICAgIFx0ZGF5TmFtZXNNaW46IFsgXCLQktGBXCIsIFwi0J/QvVwiLCBcItCS0YJcIiwgXCLQodGAXCIsIFwi0KfRglwiLCBcItCf0YJcIiwgXCLQodCxXCIgXSxcclxuICAgIFx0bW9udGhOYW1lczogWyBcItCv0L3QstCw0YDRjFwiLCBcItCk0LXQstGA0LDQu9GMXCIsIFwi0JzQsNGA0YJcIiwgXCLQkNC/0YDQtdC70YxcIiwgXCLQnNCw0LlcIiwgXCLQmNGO0L3RjFwiLCBcItCY0Y7Qu9GMXCIsIFwi0JDQstCz0YPRgdGCXCIsIFwi0KHQtdC90YLRj9Cx0YDRjFwiLCBcItCe0LrRgtGP0LHRgNGMXCIsIFwi0J3QvtGP0LHRgNGMXCIsIFwi0JTQtdC60LDQsdGA0YxcIiBdLFxyXG4gICAgXHRmaXJzdERheTogMVxyXG4gICAgfSk7XHJcbiAgICAkKCBcIiN0cmlwZWRpdG1vZGFsX19mcm9tX2RhdGVwaWNrZXJcIiApLmRhdGVwaWNrZXIoe1xyXG4gICAgXHRkYXlOYW1lc01pbjogWyBcItCS0YFcIiwgXCLQn9C9XCIsIFwi0JLRglwiLCBcItCh0YBcIiwgXCLQp9GCXCIsIFwi0J/RglwiLCBcItCh0LFcIiBdLFxyXG4gICAgXHRtb250aE5hbWVzOiBbIFwi0K/QvdCy0LDRgNGMXCIsIFwi0KTQtdCy0YDQsNC70YxcIiwgXCLQnNCw0YDRglwiLCBcItCQ0L/RgNC10LvRjFwiLCBcItCc0LDQuVwiLCBcItCY0Y7QvdGMXCIsIFwi0JjRjtC70YxcIiwgXCLQkNCy0LPRg9GB0YJcIiwgXCLQodC10L3RgtGP0LHRgNGMXCIsIFwi0J7QutGC0Y/QsdGA0YxcIiwgXCLQndC+0Y/QsdGA0YxcIiwgXCLQlNC10LrQsNCx0YDRjFwiIF0sXHJcbiAgICBcdGZpcnN0RGF5OiAxXHJcbiAgICB9KTtcclxuICAgICQoIFwiI3RyaXBlZGl0bW9kYWxfX3RvX2RhdGVwaWNrZXJcIiApLmRhdGVwaWNrZXIoe1xyXG4gICAgXHRkYXlOYW1lc01pbjogWyBcItCS0YFcIiwgXCLQn9C9XCIsIFwi0JLRglwiLCBcItCh0YBcIiwgXCLQp9GCXCIsIFwi0J/RglwiLCBcItCh0LFcIiBdLFxyXG4gICAgXHRtb250aE5hbWVzOiBbIFwi0K/QvdCy0LDRgNGMXCIsIFwi0KTQtdCy0YDQsNC70YxcIiwgXCLQnNCw0YDRglwiLCBcItCQ0L/RgNC10LvRjFwiLCBcItCc0LDQuVwiLCBcItCY0Y7QvdGMXCIsIFwi0JjRjtC70YxcIiwgXCLQkNCy0LPRg9GB0YJcIiwgXCLQodC10L3RgtGP0LHRgNGMXCIsIFwi0J7QutGC0Y/QsdGA0YxcIiwgXCLQndC+0Y/QsdGA0YxcIiwgXCLQlNC10LrQsNCx0YDRjFwiIF0sXHJcbiAgICBcdGZpcnN0RGF5OiAxXHJcbiAgICB9KTtcclxuICAgICQoIFwiI2VtcHByb2ZpbGVlZGl0X19kYXRlcGlja2VyXCIgKS5kYXRlcGlja2VyKHtcclxuICAgIFx0ZGF5TmFtZXNNaW46IFsgXCLQktGBXCIsIFwi0J/QvVwiLCBcItCS0YJcIiwgXCLQodGAXCIsIFwi0KfRglwiLCBcItCf0YJcIiwgXCLQodCxXCIgXSxcclxuICAgIFx0bW9udGhOYW1lczogWyBcItCv0L3QstCw0YDRjFwiLCBcItCk0LXQstGA0LDQu9GMXCIsIFwi0JzQsNGA0YJcIiwgXCLQkNC/0YDQtdC70YxcIiwgXCLQnNCw0LlcIiwgXCLQmNGO0L3RjFwiLCBcItCY0Y7Qu9GMXCIsIFwi0JDQstCz0YPRgdGCXCIsIFwi0KHQtdC90YLRj9Cx0YDRjFwiLCBcItCe0LrRgtGP0LHRgNGMXCIsIFwi0J3QvtGP0LHRgNGMXCIsIFwi0JTQtdC60LDQsdGA0YxcIiBdLFxyXG4gICAgXHRmaXJzdERheTogMVxyXG4gICAgfSk7XHJcbiAgICAkKCBcIiN2YWNyZXF1ZXN0X19mcm9tX2RhdGVwaWNrZXJcIiApLmRhdGVwaWNrZXIoe1xyXG4gICAgXHRkYXlOYW1lc01pbjogWyBcItCS0YFcIiwgXCLQn9C9XCIsIFwi0JLRglwiLCBcItCh0YBcIiwgXCLQp9GCXCIsIFwi0J/RglwiLCBcItCh0LFcIiBdLFxyXG4gICAgXHRtb250aE5hbWVzOiBbIFwi0K/QvdCy0LDRgNGMXCIsIFwi0KTQtdCy0YDQsNC70YxcIiwgXCLQnNCw0YDRglwiLCBcItCQ0L/RgNC10LvRjFwiLCBcItCc0LDQuVwiLCBcItCY0Y7QvdGMXCIsIFwi0JjRjtC70YxcIiwgXCLQkNCy0LPRg9GB0YJcIiwgXCLQodC10L3RgtGP0LHRgNGMXCIsIFwi0J7QutGC0Y/QsdGA0YxcIiwgXCLQndC+0Y/QsdGA0YxcIiwgXCLQlNC10LrQsNCx0YDRjFwiIF0sXHJcbiAgICBcdGZpcnN0RGF5OiAxXHJcbiAgICB9KTtcclxuICAgICQoIFwiI3ZhY3JlcXVlc3RfX3RvX2RhdGVwaWNrZXJcIiApLmRhdGVwaWNrZXIoe1xyXG4gICAgXHRkYXlOYW1lc01pbjogWyBcItCS0YFcIiwgXCLQn9C9XCIsIFwi0JLRglwiLCBcItCh0YBcIiwgXCLQp9GCXCIsIFwi0J/RglwiLCBcItCh0LFcIiBdLFxyXG4gICAgXHRtb250aE5hbWVzOiBbIFwi0K/QvdCy0LDRgNGMXCIsIFwi0KTQtdCy0YDQsNC70YxcIiwgXCLQnNCw0YDRglwiLCBcItCQ0L/RgNC10LvRjFwiLCBcItCc0LDQuVwiLCBcItCY0Y7QvdGMXCIsIFwi0JjRjtC70YxcIiwgXCLQkNCy0LPRg9GB0YJcIiwgXCLQodC10L3RgtGP0LHRgNGMXCIsIFwi0J7QutGC0Y/QsdGA0YxcIiwgXCLQndC+0Y/QsdGA0YxcIiwgXCLQlNC10LrQsNCx0YDRjFwiIF0sXHJcbiAgICBcdGZpcnN0RGF5OiAxXHJcbiAgICB9KTtcclxufSk7XHJcbndpbmRvdy5vbmxvYWQgPSBmdW5jdGlvbiAoKSB7XHJcblx0ZnVuY3Rpb24gdGFicygpIHtcclxuXHQgICAgbGV0IHRhYkxpbmtzID0gZG9jdW1lbnQuZ2V0RWxlbWVudHNCeUNsYXNzTmFtZSgncHJvZmlsZV9fdGFiJyk7XHJcblx0ICAgIGxldCB0YWJQb3N0cyA9IGRvY3VtZW50LmdldEVsZW1lbnRzQnlDbGFzc05hbWUoJ3Byb2ZpbGVfX3RhYl9wb3N0Jyk7XHJcblx0ICAgIGxldCBib3R0b21MaW5lID0gZG9jdW1lbnQuZ2V0RWxlbWVudHNCeUNsYXNzTmFtZSgncG9zdHNfX3NlYXJjaF9ib3R0b21fbGluZScpWzBdO1xyXG5cclxuXHQgICAgdGFiUG9zdHNbMF0uY2xhc3NMaXN0LmFkZCgncHJvZmlsZV9fdGFiX2FjdGl2ZScpO1xyXG5cdCAgICBmb3IgKGxldCBpID0gMDsgaSA8IHRhYkxpbmtzLmxlbmd0aDsgaSsrKSB7XHJcblx0ICAgICAgdGFiTGlua3NbaV0uYWRkRXZlbnRMaXN0ZW5lcignY2xpY2snLCBmdW5jdGlvbihlKXtcclxuXHQgICAgICAgIGUucHJldmVudERlZmF1bHQoKTtcclxuXHQgICAgICAgIGZvciAobGV0IGogPSAwOyBqIDwgdGFiTGlua3MubGVuZ3RoOyBqKyspIHtcclxuXHQgICAgICAgICAgdGFiTGlua3Nbal0uY2xhc3NMaXN0LnJlbW92ZSgncHJvZmlsZV9fdGFiX2FjdGl2ZScpO1xyXG5cdCAgICAgICAgICB0YWJQb3N0c1tqXS5jbGFzc0xpc3QucmVtb3ZlKCdwcm9maWxlX190YWJfcG9zdGFjdGl2ZScpO1xyXG5cdCAgICAgICAgfVxyXG5cdCAgICAgICAgdGFiTGlua3NbaV0uY2xhc3NMaXN0LmFkZCgncHJvZmlsZV9fdGFiX2FjdGl2ZScpO1xyXG5cdCAgICAgICAgdGFiUG9zdHNbaV0uY2xhc3NMaXN0LmFkZCgncHJvZmlsZV9fdGFiX3Bvc3RhY3RpdmUnKTtcclxuXHQgICAgICB9KVxyXG5cdCAgICB9XHJcblx0fVxyXG5cdHRhYnMoKTtcdFxyXG59XHJcbiJdfQ==
$(document).ready(function () {
    $("#empprofileedit__datepicker__birthDate").mask('00.00.0000');
    $("#empprofileedit__datepicker__hireDate").mask('00.00.0000');
    $("#empprofileedit__datepicker__dismissalDate").mask('00.00.0000');
    $("#vacrequest__from_datepicker").mask('00.00.0000');
    $("#vacrequest__to_datepicker").mask('00.00.0000');
    $("#phone").mask('(000) 000-0000');
    $("#File").on('change', function () {
        $("#fileUploadLabel").empty();
        $("#fileUploadLabel").html("Фото загружено");
    });
});

function GetDiff(fromId, toId) {
    var d1 = document.getElementById(toId).value;
    var endDate = new Date(d1.split('.')[2], d1.split('.')[1] - 1, d1.split('.')[0]);
    var d2 = document.getElementById(fromId).value;
    var beginDate = new Date(d2.split('.')[2], d2.split('.')[1] - 1, d2.split('.')[0]);
    var timeDiff = endDate.getTime() - beginDate.getTime();
    return Math.ceil(timeDiff / (1000 * 3600 * 24));
}

function CalcDiff(fromId, toId) {
    if (document.getElementById(fromId).value && document.getElementById(toId).value) {
        document.getElementById("Duration").value = GetDiff(fromId, toId);
    }
}

function ShowRequestPopup(url, id) {
    url = url + id;
    $.ajax({
        type: "Get",
        url: url,
        success: function (modal_html) {
            $('#modal_place').empty();
            $('#request_modal').modal();
            $('#modal_place').html(modal_html);
        }
    });
}
function getprofilerequests(page_number, data_type) {
    url = "/profile/" + data_type + "spage?pageNumber=" + page_number;
    $.ajax({
        type: "Get",
        url: url,
        success: function (vacations_data) {
            if ($(".profile__tabs_" + data_type).html() !== '') {
                $(".profile__tabs_" + data_type).empty();
            }
            $(".profile__tabs_" + data_type).html(vacations_data);
        }
    }
    );
}
function proccessSickleave(method, id) {
    url = "/request/" + method + "sickleave?id=" + id;
    $.ajax({
        type: "Get",
        url: url,
        success: function (modal_html) {
            $('#modal_place').empty();
            $('#request_modal').modal();
            $('#modal_place').html(modal_html);
        }
    });
}
function addAssignment() {
    url = "/admin/addassignment";
    $.ajax({
        type: "Get",
        url: url,
        success: function (modal_html) {
            $('#modal_place').empty();
            $('#request_modal').modal();
            $('#modal_place').html(modal_html);
            $("#tripMembers").select2({
                placeholder: "Выберите cотрудников..."
            });
            $("#tripMembers").rules("add", {
                required: true
            });
            $("#assignmentaddmodal_datepicker_from").datepicker({
                dayNamesMin: ["Вс", "Пн", "Вт", "Ср", "Чт", "Пт", "Сб"],
                dateFormat: 'dd.mm.yy',
                monthNames: ["Январь", "Февраль", "Март", "Апрель", "Май", "Июнь", "Июль", "Август", "Сентябрь", "Октябрь", "Ноябрь", "Декабрь"],
                firstDay: 1
            });          
            $("#assignmentaddmodal_datepicker_to").datepicker({
                dayNamesMin: ["Вс", "Пн", "Вт", "Ср", "Чт", "Пт", "Сб"],
                dateFormat: 'dd.mm.yy',
                monthNames: ["Январь", "Февраль", "Март", "Апрель", "Май", "Июнь", "Июль", "Август", "Сентябрь", "Октябрь", "Ноябрь", "Декабрь"],
                firstDay: 1
            });
            $("#assignmentaddmodal_datepicker_from").mask('00.00.0000');
            $("#assignmentaddmodal_datepicker_to").mask('00.00.0000');    
        }
    });
}
function editAssignment(id) {
    url = "/admin/editassignment?id=" + id;
    $.ajax({
        type: "Get",
        url: url,
        success: function (modal_html) {
            $('#modal_place').empty();
            $('#request_modal').modal();
            $('#modal_place').html(modal_html);
            $("#tripMembers").select2({
                placeholder: "Выберите cотрудников..."
            });
            $("#tripMembers").rules("add", {
                required: true
            });
            $("#assignmentaddmodal_datepicker_from").datepicker({
                dayNamesMin: ["Вс", "Пн", "Вт", "Ср", "Чт", "Пт", "Сб"],
                dateFormat: 'dd.mm.yy',
                monthNames: ["Январь", "Февраль", "Март", "Апрель", "Май", "Июнь", "Июль", "Август", "Сентябрь", "Октябрь", "Ноябрь", "Декабрь"], firstDay: 1
            });
            $("#assignmentaddmodal_datepicker_to").datepicker({
                dayNamesMin: ["Вс", "Пн", "Вт", "Ср", "Чт", "Пт", "Сб"],
                dateFormat: 'dd.mm.yy',
                monthNames: ["Январь", "Февраль", "Март", "Апрель", "Май", "Июнь", "Июль", "Август", "Сентябрь", "Октябрь", "Ноябрь", "Декабрь"], firstDay: 1
            });
            $("#assignmentaddmodal_datepicker_from").mask('00.00.0000');
            $("#assignmentaddmodal_datepicker_to").mask('00.00.0000');

        }
    });
}

