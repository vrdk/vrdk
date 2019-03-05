$(document).ready(function () { $(function () { $("#vacrequestmodal__from_datepicker").datepicker({ dayNamesMin: ["Вс", "Пн", "Вт", "Ср", "Чт", "Пт", "Сб"], dateFormat: 'dd.mm.yy', monthNames: ["Январь", "Февраль", "Март", "Апрель", "Май", "Июнь", "Июль", "Август", "Сентябрь", "Октябрь", "Ноябрь", "Декабрь"], firstDay: 1 }), $("#vacrequestmodal__to_datepicker").datepicker({ dayNamesMin: ["Вс", "Пн", "Вт", "Ср", "Чт", "Пт", "Сб"], dateFormat: 'dd.mm.yy', monthNames: ["Январь", "Февраль", "Март", "Апрель", "Май", "Июнь", "Июль", "Август", "Сентябрь", "Октябрь", "Ноябрь", "Декабрь"], firstDay: 1 }), $("#tripaddmodal__from_datepicker").datepicker({ dayNamesMin: ["Вс", "Пн", "Вт", "Ср", "Чт", "Пт", "Сб"], dateFormat: 'dd.mm.yy', monthNames: ["Январь", "Февраль", "Март", "Апрель", "Май", "Июнь", "Июль", "Август", "Сентябрь", "Октябрь", "Ноябрь", "Декабрь"], firstDay: 1 }), $("#tripaddmodal__to_datepicker").datepicker({ dayNamesMin: ["Вс", "Пн", "Вт", "Ср", "Чт", "Пт", "Сб"], dateFormat: 'dd.mm.yy', monthNames: ["Январь", "Февраль", "Март", "Апрель", "Май", "Июнь", "Июль", "Август", "Сентябрь", "Октябрь", "Ноябрь", "Декабрь"], firstDay: 1 }), $("#tripeditmodal__from_datepicker").datepicker({ dayNamesMin: ["Вс", "Пн", "Вт", "Ср", "Чт", "Пт", "Сб"], dateFormat: 'dd.mm.yy', monthNames: ["Январь", "Февраль", "Март", "Апрель", "Май", "Июнь", "Июль", "Август", "Сентябрь", "Октябрь", "Ноябрь", "Декабрь"], firstDay: 1 }), $("#tripeditmodal__to_datepicker").datepicker({ dayNamesMin: ["Вс", "Пн", "Вт", "Ср", "Чт", "Пт", "Сб"], dateFormat: 'dd.mm.yy', monthNames: ["Январь", "Февраль", "Март", "Апрель", "Май", "Июнь", "Июль", "Август", "Сентябрь", "Октябрь", "Ноябрь", "Декабрь"], firstDay: 1 }), $("#empprofileedit__datepicker__birthDate").datepicker({ dayNamesMin: ["Вс", "Пн", "Вт", "Ср", "Чт", "Пт", "Сб"], dateFormat: 'dd.mm.yy', monthNames: ["Январь", "Февраль", "Март", "Апрель", "Май", "Июнь", "Июль", "Август", "Сентябрь", "Октябрь", "Ноябрь", "Декабрь"], firstDay: 1 }), $("#from_datepicker").datepicker({ dayNamesMin: ["Вс", "Пн", "Вт", "Ср", "Чт", "Пт", "Сб"], dateFormat: 'dd.mm.yy', monthNames: ["Январь", "Февраль", "Март", "Апрель", "Май", "Июнь", "Июль", "Август", "Сентябрь", "Октябрь", "Ноябрь", "Декабрь"], firstDay: 1 }), $("#to_datepicker").datepicker({ dayNamesMin: ["Вс", "Пн", "Вт", "Ср", "Чт", "Пт", "Сб"], dateFormat: 'dd.mm.yy', monthNames: ["Январь", "Февраль", "Март", "Апрель", "Май", "Июнь", "Июль", "Август", "Сентябрь", "Октябрь", "Ноябрь", "Декабрь"], firstDay: 1 }), $("#empprofileedit__datepicker__hireDate").datepicker({ dayNamesMin: ["Вс", "Пн", "Вт", "Ср", "Чт", "Пт", "Сб"], dateFormat: 'dd.mm.yy', monthNames: ["Январь", "Февраль", "Март", "Апрель", "Май", "Июнь", "Июль", "Август", "Сентябрь", "Октябрь", "Ноябрь", "Декабрь"], firstDay: 1 }), $("#empprofileedit__datepicker__dismissalDate").datepicker({ dayNamesMin: ["Вс", "Пн", "Вт", "Ср", "Чт", "Пт", "Сб"], dateFormat: 'dd.mm.yy', monthNames: ["Январь", "Февраль", "Март", "Апрель", "Май", "Июнь", "Июль", "Август", "Сентябрь", "Октябрь", "Ноябрь", "Декабрь"], firstDay: 1 }); }), window.onload = function () { !function () { let e = document.getElementsByClassName("profile__tab"), a = document.getElementsByClassName("profile__tab_post"); for (let t = 0; t < e.length; t++)e[t].addEventListener("click", function (i) { i.preventDefault(); for (let t = 0; t < e.length; t++)e[t].classList.remove("profile__tab_active"), a[t].classList.remove("profile__tab_postactive"); e[t].classList.add("profile__tab_active"), a[t].classList.add("profile__tab_postactive"); }); }(); }; });
//# sourceMappingURL=data:application/json;charset=utf8;base64,eyJ2ZXJzaW9uIjozLCJzb3VyY2VzIjpbIm1haW4uanMiXSwibmFtZXMiOlsiJCIsImRhdGVwaWNrZXIiLCJkYXlOYW1lc01pbiIsIm1vbnRoTmFtZXMiLCJmaXJzdERheSIsIndpbmRvdyIsIm9ubG9hZCIsInRhYkxpbmtzIiwiZG9jdW1lbnQiLCJnZXRFbGVtZW50c0J5Q2xhc3NOYW1lIiwidGFiUG9zdHMiLCJjbGFzc0xpc3QiLCJhZGQiLCJpIiwibGVuZ3RoIiwiYWRkRXZlbnRMaXN0ZW5lciIsImUiLCJwcmV2ZW50RGVmYXVsdCIsImoiLCJyZW1vdmUiLCJ0YWJzIl0sIm1hcHBpbmdzIjoiQUFBQ0EsRUFBRyxXQUNBQSxFQUFHLHFDQUFzQ0MsV0FBVyxDQUNuREMsWUFBYSxDQUFFLEtBQU0sS0FBTSxLQUFNLEtBQU0sS0FBTSxLQUFNLE1BQ25EQyxXQUFZLENBQUUsU0FBVSxVQUFXLE9BQVEsU0FBVSxNQUFPLE9BQVEsT0FBUSxTQUFVLFdBQVksVUFBVyxTQUFVLFdBQ3ZIQyxTQUFVLElBRVhKLEVBQUcsbUNBQW9DQyxXQUFXLENBQ2pEQyxZQUFhLENBQUUsS0FBTSxLQUFNLEtBQU0sS0FBTSxLQUFNLEtBQU0sTUFDbkRDLFdBQVksQ0FBRSxTQUFVLFVBQVcsT0FBUSxTQUFVLE1BQU8sT0FBUSxPQUFRLFNBQVUsV0FBWSxVQUFXLFNBQVUsV0FDdkhDLFNBQVUsSUFFWEosRUFBRyxrQ0FBbUNDLFdBQVcsQ0FDaERDLFlBQWEsQ0FBRSxLQUFNLEtBQU0sS0FBTSxLQUFNLEtBQU0sS0FBTSxNQUNuREMsV0FBWSxDQUFFLFNBQVUsVUFBVyxPQUFRLFNBQVUsTUFBTyxPQUFRLE9BQVEsU0FBVSxXQUFZLFVBQVcsU0FBVSxXQUN2SEMsU0FBVSxJQUVYSixFQUFHLGdDQUFpQ0MsV0FBVyxDQUM5Q0MsWUFBYSxDQUFFLEtBQU0sS0FBTSxLQUFNLEtBQU0sS0FBTSxLQUFNLE1BQ25EQyxXQUFZLENBQUUsU0FBVSxVQUFXLE9BQVEsU0FBVSxNQUFPLE9BQVEsT0FBUSxTQUFVLFdBQVksVUFBVyxTQUFVLFdBQ3ZIQyxTQUFVLElBRVhKLEVBQUcsbUNBQW9DQyxXQUFXLENBQ2pEQyxZQUFhLENBQUUsS0FBTSxLQUFNLEtBQU0sS0FBTSxLQUFNLEtBQU0sTUFDbkRDLFdBQVksQ0FBRSxTQUFVLFVBQVcsT0FBUSxTQUFVLE1BQU8sT0FBUSxPQUFRLFNBQVUsV0FBWSxVQUFXLFNBQVUsV0FDdkhDLFNBQVUsSUFFWEosRUFBRyxpQ0FBa0NDLFdBQVcsQ0FDL0NDLFlBQWEsQ0FBRSxLQUFNLEtBQU0sS0FBTSxLQUFNLEtBQU0sS0FBTSxNQUNuREMsV0FBWSxDQUFFLFNBQVUsVUFBVyxPQUFRLFNBQVUsTUFBTyxPQUFRLE9BQVEsU0FBVSxXQUFZLFVBQVcsU0FBVSxXQUN2SEMsU0FBVSxJQUVYSixFQUFHLCtCQUFnQ0MsV0FBVyxDQUM3Q0MsWUFBYSxDQUFFLEtBQU0sS0FBTSxLQUFNLEtBQU0sS0FBTSxLQUFNLE1BQ25EQyxXQUFZLENBQUUsU0FBVSxVQUFXLE9BQVEsU0FBVSxNQUFPLE9BQVEsT0FBUSxTQUFVLFdBQVksVUFBVyxTQUFVLFdBQ3ZIQyxTQUFVLElBRVhKLEVBQUcsZ0NBQWlDQyxXQUFXLENBQzlDQyxZQUFhLENBQUUsS0FBTSxLQUFNLEtBQU0sS0FBTSxLQUFNLEtBQU0sTUFDbkRDLFdBQVksQ0FBRSxTQUFVLFVBQVcsT0FBUSxTQUFVLE1BQU8sT0FBUSxPQUFRLFNBQVUsV0FBWSxVQUFXLFNBQVUsV0FDdkhDLFNBQVUsSUFFWEosRUFBRyw4QkFBK0JDLFdBQVcsQ0FDNUNDLFlBQWEsQ0FBRSxLQUFNLEtBQU0sS0FBTSxLQUFNLEtBQU0sS0FBTSxNQUNuREMsV0FBWSxDQUFFLFNBQVUsVUFBVyxPQUFRLFNBQVUsTUFBTyxPQUFRLE9BQVEsU0FBVSxXQUFZLFVBQVcsU0FBVSxXQUN2SEMsU0FBVSxNQUdmQyxPQUFPQyxPQUFTLFlBQ2YsV0FDSSxJQUFJQyxFQUFXQyxTQUFTQyx1QkFBdUIsZ0JBQzNDQyxFQUFXRixTQUFTQyx1QkFBdUIscUJBQzlCRCxTQUFTQyx1QkFBdUIsNkJBQTZCLEdBRTlFQyxFQUFTLEdBQUdDLFVBQVVDLElBQUksdUJBQzFCLElBQUssSUFBSUMsRUFBSSxFQUFHQSxFQUFJTixFQUFTTyxPQUFRRCxJQUNuQ04sRUFBU00sR0FBR0UsaUJBQWlCLFFBQVMsU0FBU0MsR0FDN0NBLEVBQUVDLGlCQUNGLElBQUssSUFBSUMsRUFBSSxFQUFHQSxFQUFJWCxFQUFTTyxPQUFRSSxJQUNuQ1gsRUFBU1csR0FBR1AsVUFBVVEsT0FBTyx1QkFDN0JULEVBQVNRLEdBQUdQLFVBQVVRLE9BQU8sMkJBRS9CWixFQUFTTSxHQUFHRixVQUFVQyxJQUFJLHVCQUMxQkYsRUFBU0csR0FBR0YsVUFBVUMsSUFBSSw2QkFJbENRIiwiZmlsZSI6Im1haW4uanMiLCJzb3VyY2VzQ29udGVudCI6WyIgJCggZnVuY3Rpb24oKSB7XHJcbiAgICAkKCBcIiN2YWNyZXF1ZXN0bW9kYWxfX2Zyb21fZGF0ZXBpY2tlclwiICkuZGF0ZXBpY2tlcih7XHJcbiAgICBcdGRheU5hbWVzTWluOiBbIFwi0JLRgVwiLCBcItCf0L1cIiwgXCLQktGCXCIsIFwi0KHRgFwiLCBcItCn0YJcIiwgXCLQn9GCXCIsIFwi0KHQsVwiIF0sXHJcbiAgICBcdG1vbnRoTmFtZXM6IFsgXCLQr9C90LLQsNGA0YxcIiwgXCLQpNC10LLRgNCw0LvRjFwiLCBcItCc0LDRgNGCXCIsIFwi0JDQv9GA0LXQu9GMXCIsIFwi0JzQsNC5XCIsIFwi0JjRjtC90YxcIiwgXCLQmNGO0LvRjFwiLCBcItCQ0LLQs9GD0YHRglwiLCBcItCh0LXQvdGC0Y/QsdGA0YxcIiwgXCLQntC60YLRj9Cx0YDRjFwiLCBcItCd0L7Rj9Cx0YDRjFwiLCBcItCU0LXQutCw0LHRgNGMXCIgXSxcclxuICAgIFx0Zmlyc3REYXk6IDFcclxuICAgIH0pO1xyXG4gICAgJCggXCIjdmFjcmVxdWVzdG1vZGFsX190b19kYXRlcGlja2VyXCIgKS5kYXRlcGlja2VyKHtcclxuICAgIFx0ZGF5TmFtZXNNaW46IFsgXCLQktGBXCIsIFwi0J/QvVwiLCBcItCS0YJcIiwgXCLQodGAXCIsIFwi0KfRglwiLCBcItCf0YJcIiwgXCLQodCxXCIgXSxcclxuICAgIFx0bW9udGhOYW1lczogWyBcItCv0L3QstCw0YDRjFwiLCBcItCk0LXQstGA0LDQu9GMXCIsIFwi0JzQsNGA0YJcIiwgXCLQkNC/0YDQtdC70YxcIiwgXCLQnNCw0LlcIiwgXCLQmNGO0L3RjFwiLCBcItCY0Y7Qu9GMXCIsIFwi0JDQstCz0YPRgdGCXCIsIFwi0KHQtdC90YLRj9Cx0YDRjFwiLCBcItCe0LrRgtGP0LHRgNGMXCIsIFwi0J3QvtGP0LHRgNGMXCIsIFwi0JTQtdC60LDQsdGA0YxcIiBdLFxyXG4gICAgXHRmaXJzdERheTogMVxyXG4gICAgfSk7XHJcbiAgICAkKCBcIiN0cmlwYWRkbW9kYWxfX2Zyb21fZGF0ZXBpY2tlclwiICkuZGF0ZXBpY2tlcih7XHJcbiAgICBcdGRheU5hbWVzTWluOiBbIFwi0JLRgVwiLCBcItCf0L1cIiwgXCLQktGCXCIsIFwi0KHRgFwiLCBcItCn0YJcIiwgXCLQn9GCXCIsIFwi0KHQsVwiIF0sXHJcbiAgICBcdG1vbnRoTmFtZXM6IFsgXCLQr9C90LLQsNGA0YxcIiwgXCLQpNC10LLRgNCw0LvRjFwiLCBcItCc0LDRgNGCXCIsIFwi0JDQv9GA0LXQu9GMXCIsIFwi0JzQsNC5XCIsIFwi0JjRjtC90YxcIiwgXCLQmNGO0LvRjFwiLCBcItCQ0LLQs9GD0YHRglwiLCBcItCh0LXQvdGC0Y/QsdGA0YxcIiwgXCLQntC60YLRj9Cx0YDRjFwiLCBcItCd0L7Rj9Cx0YDRjFwiLCBcItCU0LXQutCw0LHRgNGMXCIgXSxcclxuICAgIFx0Zmlyc3REYXk6IDFcclxuICAgIH0pO1xyXG4gICAgJCggXCIjdHJpcGFkZG1vZGFsX190b19kYXRlcGlja2VyXCIgKS5kYXRlcGlja2VyKHtcclxuICAgIFx0ZGF5TmFtZXNNaW46IFsgXCLQktGBXCIsIFwi0J/QvVwiLCBcItCS0YJcIiwgXCLQodGAXCIsIFwi0KfRglwiLCBcItCf0YJcIiwgXCLQodCxXCIgXSxcclxuICAgIFx0bW9udGhOYW1lczogWyBcItCv0L3QstCw0YDRjFwiLCBcItCk0LXQstGA0LDQu9GMXCIsIFwi0JzQsNGA0YJcIiwgXCLQkNC/0YDQtdC70YxcIiwgXCLQnNCw0LlcIiwgXCLQmNGO0L3RjFwiLCBcItCY0Y7Qu9GMXCIsIFwi0JDQstCz0YPRgdGCXCIsIFwi0KHQtdC90YLRj9Cx0YDRjFwiLCBcItCe0LrRgtGP0LHRgNGMXCIsIFwi0J3QvtGP0LHRgNGMXCIsIFwi0JTQtdC60LDQsdGA0YxcIiBdLFxyXG4gICAgXHRmaXJzdERheTogMVxyXG4gICAgfSk7XHJcbiAgICAkKCBcIiN0cmlwZWRpdG1vZGFsX19mcm9tX2RhdGVwaWNrZXJcIiApLmRhdGVwaWNrZXIoe1xyXG4gICAgXHRkYXlOYW1lc01pbjogWyBcItCS0YFcIiwgXCLQn9C9XCIsIFwi0JLRglwiLCBcItCh0YBcIiwgXCLQp9GCXCIsIFwi0J/RglwiLCBcItCh0LFcIiBdLFxyXG4gICAgXHRtb250aE5hbWVzOiBbIFwi0K/QvdCy0LDRgNGMXCIsIFwi0KTQtdCy0YDQsNC70YxcIiwgXCLQnNCw0YDRglwiLCBcItCQ0L/RgNC10LvRjFwiLCBcItCc0LDQuVwiLCBcItCY0Y7QvdGMXCIsIFwi0JjRjtC70YxcIiwgXCLQkNCy0LPRg9GB0YJcIiwgXCLQodC10L3RgtGP0LHRgNGMXCIsIFwi0J7QutGC0Y/QsdGA0YxcIiwgXCLQndC+0Y/QsdGA0YxcIiwgXCLQlNC10LrQsNCx0YDRjFwiIF0sXHJcbiAgICBcdGZpcnN0RGF5OiAxXHJcbiAgICB9KTtcclxuICAgICQoIFwiI3RyaXBlZGl0bW9kYWxfX3RvX2RhdGVwaWNrZXJcIiApLmRhdGVwaWNrZXIoe1xyXG4gICAgXHRkYXlOYW1lc01pbjogWyBcItCS0YFcIiwgXCLQn9C9XCIsIFwi0JLRglwiLCBcItCh0YBcIiwgXCLQp9GCXCIsIFwi0J/RglwiLCBcItCh0LFcIiBdLFxyXG4gICAgXHRtb250aE5hbWVzOiBbIFwi0K/QvdCy0LDRgNGMXCIsIFwi0KTQtdCy0YDQsNC70YxcIiwgXCLQnNCw0YDRglwiLCBcItCQ0L/RgNC10LvRjFwiLCBcItCc0LDQuVwiLCBcItCY0Y7QvdGMXCIsIFwi0JjRjtC70YxcIiwgXCLQkNCy0LPRg9GB0YJcIiwgXCLQodC10L3RgtGP0LHRgNGMXCIsIFwi0J7QutGC0Y/QsdGA0YxcIiwgXCLQndC+0Y/QsdGA0YxcIiwgXCLQlNC10LrQsNCx0YDRjFwiIF0sXHJcbiAgICBcdGZpcnN0RGF5OiAxXHJcbiAgICB9KTtcclxuICAgICQoIFwiI2VtcHByb2ZpbGVlZGl0X19kYXRlcGlja2VyXCIgKS5kYXRlcGlja2VyKHtcclxuICAgIFx0ZGF5TmFtZXNNaW46IFsgXCLQktGBXCIsIFwi0J/QvVwiLCBcItCS0YJcIiwgXCLQodGAXCIsIFwi0KfRglwiLCBcItCf0YJcIiwgXCLQodCxXCIgXSxcclxuICAgIFx0bW9udGhOYW1lczogWyBcItCv0L3QstCw0YDRjFwiLCBcItCk0LXQstGA0LDQu9GMXCIsIFwi0JzQsNGA0YJcIiwgXCLQkNC/0YDQtdC70YxcIiwgXCLQnNCw0LlcIiwgXCLQmNGO0L3RjFwiLCBcItCY0Y7Qu9GMXCIsIFwi0JDQstCz0YPRgdGCXCIsIFwi0KHQtdC90YLRj9Cx0YDRjFwiLCBcItCe0LrRgtGP0LHRgNGMXCIsIFwi0J3QvtGP0LHRgNGMXCIsIFwi0JTQtdC60LDQsdGA0YxcIiBdLFxyXG4gICAgXHRmaXJzdERheTogMVxyXG4gICAgfSk7XHJcbiAgICAkKCBcIiN2YWNyZXF1ZXN0X19mcm9tX2RhdGVwaWNrZXJcIiApLmRhdGVwaWNrZXIoe1xyXG4gICAgXHRkYXlOYW1lc01pbjogWyBcItCS0YFcIiwgXCLQn9C9XCIsIFwi0JLRglwiLCBcItCh0YBcIiwgXCLQp9GCXCIsIFwi0J/RglwiLCBcItCh0LFcIiBdLFxyXG4gICAgXHRtb250aE5hbWVzOiBbIFwi0K/QvdCy0LDRgNGMXCIsIFwi0KTQtdCy0YDQsNC70YxcIiwgXCLQnNCw0YDRglwiLCBcItCQ0L/RgNC10LvRjFwiLCBcItCc0LDQuVwiLCBcItCY0Y7QvdGMXCIsIFwi0JjRjtC70YxcIiwgXCLQkNCy0LPRg9GB0YJcIiwgXCLQodC10L3RgtGP0LHRgNGMXCIsIFwi0J7QutGC0Y/QsdGA0YxcIiwgXCLQndC+0Y/QsdGA0YxcIiwgXCLQlNC10LrQsNCx0YDRjFwiIF0sXHJcbiAgICBcdGZpcnN0RGF5OiAxXHJcbiAgICB9KTtcclxuICAgICQoIFwiI3ZhY3JlcXVlc3RfX3RvX2RhdGVwaWNrZXJcIiApLmRhdGVwaWNrZXIoe1xyXG4gICAgXHRkYXlOYW1lc01pbjogWyBcItCS0YFcIiwgXCLQn9C9XCIsIFwi0JLRglwiLCBcItCh0YBcIiwgXCLQp9GCXCIsIFwi0J/RglwiLCBcItCh0LFcIiBdLFxyXG4gICAgXHRtb250aE5hbWVzOiBbIFwi0K/QvdCy0LDRgNGMXCIsIFwi0KTQtdCy0YDQsNC70YxcIiwgXCLQnNCw0YDRglwiLCBcItCQ0L/RgNC10LvRjFwiLCBcItCc0LDQuVwiLCBcItCY0Y7QvdGMXCIsIFwi0JjRjtC70YxcIiwgXCLQkNCy0LPRg9GB0YJcIiwgXCLQodC10L3RgtGP0LHRgNGMXCIsIFwi0J7QutGC0Y/QsdGA0YxcIiwgXCLQndC+0Y/QsdGA0YxcIiwgXCLQlNC10LrQsNCx0YDRjFwiIF0sXHJcbiAgICBcdGZpcnN0RGF5OiAxXHJcbiAgICB9KTtcclxufSk7XHJcbndpbmRvdy5vbmxvYWQgPSBmdW5jdGlvbiAoKSB7XHJcblx0ZnVuY3Rpb24gdGFicygpIHtcclxuXHQgICAgbGV0IHRhYkxpbmtzID0gZG9jdW1lbnQuZ2V0RWxlbWVudHNCeUNsYXNzTmFtZSgncHJvZmlsZV9fdGFiJyk7XHJcblx0ICAgIGxldCB0YWJQb3N0cyA9IGRvY3VtZW50LmdldEVsZW1lbnRzQnlDbGFzc05hbWUoJ3Byb2ZpbGVfX3RhYl9wb3N0Jyk7XHJcblx0ICAgIGxldCBib3R0b21MaW5lID0gZG9jdW1lbnQuZ2V0RWxlbWVudHNCeUNsYXNzTmFtZSgncG9zdHNfX3NlYXJjaF9ib3R0b21fbGluZScpWzBdO1xyXG5cclxuXHQgICAgdGFiUG9zdHNbMF0uY2xhc3NMaXN0LmFkZCgncHJvZmlsZV9fdGFiX2FjdGl2ZScpO1xyXG5cdCAgICBmb3IgKGxldCBpID0gMDsgaSA8IHRhYkxpbmtzLmxlbmd0aDsgaSsrKSB7XHJcblx0ICAgICAgdGFiTGlua3NbaV0uYWRkRXZlbnRMaXN0ZW5lcignY2xpY2snLCBmdW5jdGlvbihlKXtcclxuXHQgICAgICAgIGUucHJldmVudERlZmF1bHQoKTtcclxuXHQgICAgICAgIGZvciAobGV0IGogPSAwOyBqIDwgdGFiTGlua3MubGVuZ3RoOyBqKyspIHtcclxuXHQgICAgICAgICAgdGFiTGlua3Nbal0uY2xhc3NMaXN0LnJlbW92ZSgncHJvZmlsZV9fdGFiX2FjdGl2ZScpO1xyXG5cdCAgICAgICAgICB0YWJQb3N0c1tqXS5jbGFzc0xpc3QucmVtb3ZlKCdwcm9maWxlX190YWJfcG9zdGFjdGl2ZScpO1xyXG5cdCAgICAgICAgfVxyXG5cdCAgICAgICAgdGFiTGlua3NbaV0uY2xhc3NMaXN0LmFkZCgncHJvZmlsZV9fdGFiX2FjdGl2ZScpO1xyXG5cdCAgICAgICAgdGFiUG9zdHNbaV0uY2xhc3NMaXN0LmFkZCgncHJvZmlsZV9fdGFiX3Bvc3RhY3RpdmUnKTtcclxuXHQgICAgICB9KVxyXG5cdCAgICB9XHJcblx0fVxyXG5cdHRhYnMoKTtcdFxyXG59XHJcbiJdfQ==
$(document).ready(function () {
    $("#empprofileedit__datepicker__birthDate").mask('00.00.0000');
    $("#empprofileedit__datepicker__hireDate").mask('00.00.0000');
    $("#empprofileedit__datepicker__dismissalDate").mask('00.00.0000');
    $("#from_datepicker").mask('00.00.0000');
    $("#to_datepicker").mask('00.00.0000');
    $("#phone").mask('(000) 000-0000');
    $("#File").on('change', function () {
        var inputFile = $(this);
        if (inputFile.val() === "") {
            $("#fileUploadText").html("Добавить фото");
        }
        else {
            $("#fileUploadText").html("Фото загружено");
        }
    });
    $("#Photo").on('focus', function () {
        $(this).val("");
    });
    $("a").on('click', function () {
        showPreloader();
    });
    $(".submit_button").on('click', function () {
        showPreloader();
    });
    $(".back_button").on('click', function () {
        showPreloader();
        window.location.href = document.referrer;
    });
    $(".date_picker_diff").on('change', function () {
        var input = $(this);
        var date = parseDate(input.val());
        input.val(date);
        CalcDiff('from_datepicker', 'to_datepicker');
    });
    $(".date_picker").on('change', function () {
        var input = $(this);
        var date = parseDate(input.val());
        input.val(date);
    });
    ligthMenuItem();
    $('.content').on('click', '.list_paggination', function () {
        var target = $(this);
        var action = target.attr('action-anchor');
        var page = target.attr('pageNumber-anchor');
        var searchKey = target.attr('searchKey-anchor');
        $('#paggination_list').css('display', 'none');
        $('#paggination_list_gif').css('display', 'flex');
        $.ajax({
            url: action,
            method: 'get',
            data: {
                pageNumber: page,
                searchKey: searchKey
            },
            success: function (page_html) {
                $('.listsection').replaceWith(page_html);
            },
            error: function () {
                alert('pagination error');
            }
        });
    });
    $('#list_form').on('submit', function (event) {
        showPreloader();
        event.preventDefault();
        var form = $(this);
        var data = form.serializeArray();
        $.ajax({
            url: form.attr('action'),
            method: 'get',
            data: data,
            success: function (search_result_html) {
                $('.listsection').replaceWith(search_result_html);
                closePreloader();
            },
            error: function () {
                alert('search error');
                closePreloader();
            }
        });
    });
    $("#notes-new").on('change', function () {
        var icon = $(this);
        var url = "/profile/checknotificationsnuvelty";
        $.ajax({
            url: url,
            data: {
                userEmail: icon.attr('user-notifications-anchor')
            },
            method: 'get',
            success: function (result) {
                if (result) {
                    icon.css('display', 'inline-block');
                }
            }
        });
    });
    $("#notes-new").trigger('change');
});
function ligthMenuItem() {
    var menu = $('.navigation__link');
    var url = location.href;
    url = url.split('/');
    if (url.length > 4) {
        for (var i = 0; i <= menu.length - 1; i++) {
            if (url[url.length - 1].toLowerCase().indexOf(menu[i].id) >= 0) {
                menu[i].classList.add('navigation__link_active');
            }
            else {
                menu[i].classList.remove('navigation__link_active');
            }
        }
    }
}

function parseDate(date) {
    if (date) {
        var today = new Date();
        var regex = /^\d+$/;
        var isValid = true;
        var stringDate = date.split('.');
        var month = stringDate[1];
        if (month && regex.test(month)) {
            var intMonth = parseInt(month);

            if (intMonth > 12) {
                month = 12;
            } else {
                if (intMonth <= 0) {
                    month = 1;
                }
            }
        } else if (month) {
            month = today.getMonth();
        } else {
            isValid = false;
        }
        var year = stringDate[2];
        if (year && regex.test(year)) {
            if (parseInt(year) < 100) {
                year = today.getFullYear();
            }
        }
        else if (year) {
            year = today.getFullYear();
        }
        if (!year) {
            isValid = false;
        }
        var day = stringDate[0];
        if (day && regex.test(day)) {
            if (isValid) {
                var currDate = new Date(parseInt(year), parseInt(month + 1), 0);
                if (day > currDate.getDate() || day <= 0) {
                    day = currDate.getDate();
                }
            }
        }
        else if (day) {
            day = today.getDate();
        }
        else {
            isValid = false;
        }

        if (isValid) {
            return day + '.' + month + '.' + year;
        }
        else {
            var corrcetMonth = parseInt(today.getMonth()) + 1;
            return today.getDate() + '.' + corrcetMonth + '.' + today.getFullYear();
        }
    }
}

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

function changeUserPhoto(input) {

    if (input.files && input.files[0]) {
        var reader = new FileReader();
        reader.onload = function (e) {
            $('#user_photo').attr('src', e.target.result);
        };
        reader.readAsDataURL(input.files[0]);
    }
}

function ShowRequestPopup(url, id) {
    var preloader = $("#preloader");
    preloader.css('display', 'flex');
    url = url + id;
    $.ajax({
        type: "Get",
        url: url,
        success: function (modal_html) {
            $('#modal_place').empty();
            $('#request_modal').modal();
            $('#modal_place').html(modal_html);
            preloader.css('display', 'none');
        },
        error: function () {
            preloader.css('display', 'none');
        }
    });

}


function proccessSickleave(method, id) {
    var preloader = $("#preloader");
    preloader.css('display', 'flex');
    url = "/request/" + method + "sickleave?id=" + id;
    $.ajax({
        type: "Get",
        url: url,
        success: function (modal_html) {
            $('#modal_place').empty();
            $('#request_modal').modal();
            $('#modal_place').html(modal_html);
            preloader.css('display', 'none');
        },
        error: function () {
            preloader.css('display', 'none');
        }
    });


}

function deleteTeam(id) {
    var preloader = $("#preloader");
    preloader.css('display', 'flex');
    var url = "/admin/deleteteam";
    $.ajax({
        url: url,
        method: 'get',
        data: {
            id: id
        },
        success: function (modal_html) {
            $('#modal_place').empty();
            $('#request_modal').modal();
            $('#modal_place').html(modal_html);
            preloader.css('display', 'none');
        },
        error: function () {
            preloader.css('display', 'none');
        }
    });
}

function deleteAssignment(id) {
    var preloader = $("#preloader");
    preloader.css('display', 'flex');
    var url = "/admin/deleteassignment";
    $.ajax({
        url: url,
        method: 'get',
        data: {
            id: id
        },
        success: function (modal_html) {
            $('#modal_place').empty();
            $('#request_modal').modal();
            $('#modal_place').html(modal_html);
            preloader.css('display', 'none');
        },
        error: function () {
            preloader.css('display', 'none');
        }
    });
}

function showPreloader() {
    var pl = $('#preloader');
    pl.css('display', 'flex');
}

function closePreloader() {
    $('#preloader').css('display', 'none');
}

function manageMenu() {
    var menu = $(".header__burger");
    if (menu.css('visibility') === 'hidden') {
        menu.css('visibility', 'visible');
        menu.css('opacity', '1');
        $(document.body).on('click', function () {
            manageMenu();
        });
    }
    else {
        menu.css('visibility', 'hidden');
        menu.css('opacity', '0');
        $(document.body).off('click');
    }
}
