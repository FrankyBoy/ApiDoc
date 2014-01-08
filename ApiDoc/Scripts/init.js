function init(){ 
    tinyMCE.init({

        // General options
        plugins: "table preview visualblocks code",
        menubar: "edit view format table tools",
        selector: "textarea.richtext",
        tools: "inserttable",
        verify_html: false,
    });
};
