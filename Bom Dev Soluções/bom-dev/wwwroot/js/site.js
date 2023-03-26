// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
$('.dropdown-menu a.dropdown-toggle').on('click', function(e) {
    if (!$(this).next().hasClass('show')) {
      $(this).parents('.dropdown-menu').first().find('.show').removeClass("show");
    }
    var $subMenu = $(this).next(".dropdown-menu");
    $subMenu.toggleClass('show');
  
  
    $(this).parents('li.nav-item.dropdown.show').on('hidden.bs.dropdown', function(e) {
      $('.dropdown-submenu .show').removeClass("show");
    });
    
    return false;
});

function loadingShow() {
    var loading = $("#divProgress");

    loading.fadeIn();
}

function loadingClose() {
    var loading = $("#divProgress");

    loading.fadeOut();
}

function gerarTrilhaDeNavegacao(pathStr, pathToBack) {
    var niveis = pathStr.split('/');
    const items = [];

    for (let i = 0; i < niveis.length; i++) {
        if (i === niveis.length - 1) {
            items.push('<li class="breadcrumb-item active" aria-current="page">' + niveis[i] + '</li>');
        } else if (i === niveis.length - 2) {
            items.push('<li class="breadcrumb-item"><a href="#" onclick=\"menuShowCategories(\'' + pathToBack + '\', this); return false;\">' + niveis[i] + '</a></li>');
        }
    }

    return '<nav aria-label="breadcrumb" class="menuBreadcrumb"><ol class="breadcrumb">' + items.join('') + '</ol></nav>';
}



//function menuShowCategories(path, sender, preventPopUpShow = true) {
    
//    var parent = sender.closest('.dropdown-menu');
    
//    loadingShow();
        
//    $.ajax({
//        type: 'POST',
//        url: '/ViewComponent/GetCategoriesByParentIdFromPath',
//        data: jQuery.param({ path: path }),
//        cache: false,
//        success: (e) => {
//            var first = true;
//            parent.innerHTML = '';
//            $.each(e, function (index) {                
//                var name = e[index].name;
//                var pathItem = e[index].path;
//                var url = e[index].url;
//                const arr = path.split("/");
//                const arrLen = arr.length;
//                const penultimo = arrLen - 1;

//                if (penultimo <= 0) {
//                    // chegou na raiz onde não tem mais categorias filhos

//                    first = false;

//                    parent.innerHTML = gerarTrilhaDeNavegacao(path,null);
//                }                
           
//                if (first) {
//                    const antepenultimo = arr.slice(0, -1);
//                    const stringPenultimo = antepenultimo.join("/");                    

//                    parent.innerHTML = gerarTrilhaDeNavegacao(path, stringPenultimo);                    

//                    first = false;
//                }

//                if (url == null || url == '') {
//                    parent.innerHTML += '<a class=\"dropdown-item\" onclick=\"menuShowCategories(\'' + pathItem + '\', this);return false;\" href=\"#\">' + name + '&nbsp;&nbsp;&nbsp;<span class=\"caret\"></span></a>';
//                }
//                else {
//                    parent.innerHTML += '<a class=\"dropdown-item\" href=\"' + url + '\">' + name + '</a>';
//                }                
//            });
//        },
//        error: (e) => {
//            loadingClose();
//        },
//        complete: (e) => {            
//            loadingClose();
//            if (preventPopUpShow) {
//                // mantem o pop up aberto, exceto quando o onclick esta no botao
//                parent.previousElementSibling.click();
//            }
            
//        }
//    });
//}

function menuShowCategories(path, sender) {
    const parent = sender.closest('.dropdown-menu');
    const loading = loadingShow();

    $.ajax({
        type: 'POST',
        url: '/ViewComponent/GetCategoriesByParentIdFromPath',
        data: jQuery.param({ path }),
        cache: false,
        success: (categories) => {
            let first = true;
            let html = '';

            const arr = path.split("/");
            const arrLen = arr.length;
            const penultimo = arrLen - 1;

            if (penultimo <= 0) {
                // chegou na raiz onde não tem mais categorias filhos
                first = false;
                html = gerarTrilhaDeNavegacao(path, null);
            }

            if (first) {
                const antepenultimo = arr.slice(0, -1);
                const stringPenultimo = antepenultimo.join("/");
                html = gerarTrilhaDeNavegacao(path, stringPenultimo);
            }

            categories.forEach(({ name, path: pathItem, url }) => {
                if (url === null || url === '') {
                    html += `
                        <a class="dropdown-item" onclick="menuShowCategories('${pathItem}', this);return false;" href="#">
                          ${name}&nbsp;&nbsp;&nbsp;<span class="caret"></span>
                        </a>`;
                } else {
                    html += `<a class="dropdown-item" href="${url}">${name}</a>`;
                }
            });

            parent.innerHTML = html;
        },
        error: () => { },
        complete: () => {
            loadingClose(loading);
            parent.previousElementSibling.click();
        },
    });
}
