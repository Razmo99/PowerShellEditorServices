$a = 1..5
$b = 6..10
function test {
    process {

        for ($i = 0; $i -lt $a.Count; $i++) {
            $i
        }

        for ($i = 0; $i -lt $a.Count; $i++) {
            $i
        }
    }
}
