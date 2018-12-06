module Challenge02 (run) where

    import Data.List
    import Control.Monad

    run :: IO ()
    run = do
        ids <- readIds "src/02/input"
        putStrLn $ show $ task1 2 ids * task1 3 ids
        putStrLn $ task2 ids

    -- Task 1
    task1 :: Int -> [String] -> Int
    task1 n = length . filter (any $ (== n) . length) . map (group . sort)

    -- Task 2
    task2 :: [String] -> String
    task2 xs = (map fst . filter ((==EQ) . cmp) . head . filter ((== 1) . length . filter ((/=EQ) . cmp)) . map split . clean . cartesian xs) xs

    -- Read lines from the source file
    readIds :: String -> IO [String]
    readIds name = lines <$> readFile name

    -- Cartesian product of two lists
    cartesian :: [a] -> [b] -> [(a, b)]
    cartesian = liftM2 (,)

    -- Compares two elements from tuple
    cmp :: Ord a => (a,a) -> Ordering
    cmp (x,y) = compare x y

    -- Removes reversed and same elements from list of tuples
    clean :: Ord a => [(a, a)] -> [(a, a)]
    clean = filter $ (==LT) . cmp

    -- Splits tuples of array to array of tuples
    split :: ([a],[a]) -> [(a, a)]
    split ([],[]) = []
    split (x:xs,y:ys) = [(x,y)] ++ split (xs,ys)