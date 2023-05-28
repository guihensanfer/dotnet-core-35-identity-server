// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

$(document).ready(function () {
    var menu = document.getElementById("menu");
        
    if (menu != null) {
        var container = document.getElementsByClassName('containerAfter')[0];
        const defaultMargin = 0;
        var menuClientHeight = (menu.clientHeight + defaultMargin) + 'px';

        if (container != null) {
            container.style.paddingTop = menuClientHeight;
        }        
    }
});
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

function gerarTrilhaDeNavegacao(categoryId, pathStr, pathToBack) {
    var niveis = pathStr.split('/');
    const items = [];

    for (let i = 0; i < niveis.length; i++) {
        if (i === niveis.length - 1) {
            items.push('<li class="breadcrumb-item active" aria-current="page">' + niveis[i] + '</li>');
        } else if (i === niveis.length - 2) {
            items.push('<li class="breadcrumb-item"><a href="#" onclick=\"handleClick(event); menuShowCategories(' + categoryId + ',\'' + pathToBack + '\', this, null, event); return false;\">' + niveis[i] + '</a></li>');
        }
    }

    return '<nav aria-label="breadcrumb" class="menuBreadcrumb"><ol class="breadcrumb">' + items.join('') + '</ol></nav>';
}

function handleClick(e, id) {
    // console.log("e.Target", e.currentTarget.textContent);
    console.log("e.Target", e);
    e.stopPropagation(); // Or e.preventDefault();
}

//$("#teste .dropdown-item").click(function (e) {
//    e.stopPropagation();
//});

function menuShowCategories(categoryId, path, sender, backCategoryId, e) {    
    e.stopPropagation();
    e.preventDefault();

    const parent = sender.closest('.dropdown-menu');
    const loading = loadingShow();

    $.ajax({
        type: 'POST',
        url: '/ViewComponent/GetCategoriesByParentId',
        data: jQuery.param({ parentCategoryId: categoryId }),
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
                html = gerarTrilhaDeNavegacao(backCategoryId, path, categoryId);
            }

            if (first) {
                const antepenultimo = arr.slice(0, -1);
                const stringPenultimo = antepenultimo.join("/");
                html = gerarTrilhaDeNavegacao(backCategoryId,path, stringPenultimo);
            }

            categories.forEach(({ categoryId: categoryIdItem, name, path: pathItem, url, parentCategoryId }) => {
                if (url === null || url === '') {
                    html += `
                        <a class="dropdown-item" onclick="menuShowCategories(${categoryIdItem},'${pathItem}', this,${parentCategoryId}, event);return false;" href="#">
                          ${name}</span>
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
            //parent.previousElementSibling.click();
        },
    });
}
