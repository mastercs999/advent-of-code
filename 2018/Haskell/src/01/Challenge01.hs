module Challenge01 (run) where

    import Control.Monad
    import Data.Set (empty, member, insert, Set)

    run :: IO ()
    run = do
        numbers <- readSequence "src/01/input"
        putStrLn $ (show . task1) numbers
        putStrLn $ (show . task2) numbers

    -- Task 1
    task1 :: [Int] -> Int
    task1 xs = sum xs

    -- Task 2
    task2 :: [Int] -> Int
    task2 xs = sumWhile (cycle xs) 0 empty where
        sumWhile :: (Num a, Ord a) => [a] -> a -> Set a -> a
        sumWhile (x:xs) suma seen
                 | member suma seen = suma
                 | otherwise        = sumWhile xs (x + suma) (insert suma seen)

    -- Read lines from the source file and converts to int
    readSequence :: String -> IO [Int]
    readSequence name = map read <$> (lines . removePlus <$> readFile name)

    -- Removes plus sign from the string
    removePlus :: String -> String
    removePlus xs = [x | x <- xs, x /= '+']