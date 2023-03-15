function prob_estim_AB=toate_rosii(N = 5000)
  % AintersectatB: toate cele 3 bile extrase sunt rosii
  count_AB = 0;
  for i=1:N
    extragere = randsample('rrrrraaavv',3,replacement=false);
    % if ismember('r', extragere)
    if extragere(1)=='r' && extragere(2)=='r' && extragere(3)=='r'
      count_AB++;
    endif
  endfor
  prob_estim_AB = count_AB/N;
 endfunction
 
 % teoretic, C de 5 luate cate 3/C de 10 luate cate 3