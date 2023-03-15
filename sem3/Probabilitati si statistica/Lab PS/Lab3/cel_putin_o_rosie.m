function prob_estim_A=cel_putin_o_rosie(N = 5000)
  % A: cel putin o bila rosie din cele 3 extrase
  count_A = 0;
  for i=1:N
    extragere = randsample('rrrrraaavv',3,replacement=false);
    % if ismember('r', extragere)
    if extragere(1)=='r' || extragere(2)=='r' || extragere(3)=='r'
      count_A++;
    endif
  endfor
  prob_estim_A = count_A/N;
 endfunction
 
 % teoretic, 1-(C5 cate 3)/C 10 cate 3