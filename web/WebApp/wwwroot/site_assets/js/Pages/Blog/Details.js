$('document').ready(function () {
    let cookieName = `blog_visit_${$('#blog-id').val()}`;
    Cookies.set(cookieName, true, { expires: 365 })
});