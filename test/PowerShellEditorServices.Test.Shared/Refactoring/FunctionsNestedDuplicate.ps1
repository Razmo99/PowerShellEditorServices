function Outer {
    write-host "Hello World"

    function Inner {
        write-host "Hello World"
    }
    Inner

}

function Inner {
        write-host "Hello World I am Diff"
}

Inner
