var scrollPos = {x: 0, y: 0};
var ticking = false;

function requestScrollAnimation(callback) {
    window.addEventListener('scroll', (e) => {
    
        scrollPos = {
            x: window.scrollX || window.pageXOffset,
            y: window.scrollY || window.pageYOffset
        };
    
        if (!ticking) {
            
            window.requestAnimationFrame((t) => {
                callback(scrollPos);
                ticking = false;
            });
            
            ticking = true;
        }
      
    });
}

function isScrolledIntoView(el, isCompleteVisible = false) {
    var rect = el.getBoundingClientRect();
    var elTop = rect.top;
    var elBottom = rect.bottom;

    var isVisible;

    if (isCompleteVisible) {
        isVisible = (elTop >= 0) && (elBottom <= window.innerHeight);
    }
    else {
        isVisible = (elTop < window.innerHeight) && (elBottom >= 0);
    }

    return isVisible;
}

function translateElement(el, x, y) {
    var transform = `translate(${x}px, ${y}px)`;
    transformElement(el, transform);
}

function transformElement(el, transform) {
    el.style.webkitTransform = transform;
    el.style.MozTransform = transform;
    el.style.msTransform = transform;
    el.style.OTransform = transform;
    el.style.transform = transform;
}