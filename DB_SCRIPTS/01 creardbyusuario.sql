
DELIMITER ;
CREATE DATABASE notasmiumg;

USE notasmiumg;

CREATE USER 'appnotasmiumg'@'%' IDENTIFIED BY '6y$EL&uk';

GRANT ALL PRIVILEGES ON notasmiumg.* TO 'appnotasmiumg'@'%';

# DROP USER appnotasmiumg;
# DROP DATABASE notasmiumg;
