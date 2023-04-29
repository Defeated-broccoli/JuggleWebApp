const triggers = document.querySelectorAll('.layout__a');
const highlight = document.createElement('span');

highlight.classList.add('layout__highlight');
document.body.append(highlight);

function handleEnter() {
    const linkCoords = this.getBoundingClientRect();

    highlight.style.width = `${linkCoords.width + 20}px`;
    highlight.style.height = `${linkCoords.height}px`;

    highlight.style.transform = `translate(${linkCoords.left - 10 + window.scrollX}px, ${linkCoords.top + window.scrollY}px)`;

    this.style.color = 'Black';
}

function handleLeave() {
    highlight.style.width = '5px';
    this.style.color = 'Whitesmoke';
}

triggers.forEach(trigger => trigger.addEventListener('mouseover', handleEnter));
triggers.forEach(trigger => trigger.addEventListener('mouseleave', handleLeave));
