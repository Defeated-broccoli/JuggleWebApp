const sidebarButton = document.querySelector('.home-index__menu-button');
const sidebar = document.querySelector('.home-index__sidebar');
const sidecontent = document.querySelector('.home-index__sidecontent');

function handleSidebarButtonClick() {
    if (!sidebar.classList.contains('home-index__sidebar-roll')) {
        sidebar.classList.add('home-index__sidebar-roll');
        setTimeout(() => {
            sidecontent.classList.add('home-index__sidecontent-visible');
        }, 500);
    }
    else {
        sidebar.classList.remove('home-index__sidebar-roll');
        sidecontent.classList.remove('home-index__sidecontent-visible');
    }
}

sidebarButton.addEventListener('click', handleSidebarButtonClick);